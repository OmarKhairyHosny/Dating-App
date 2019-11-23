using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Dtos.Photo;
using DatingApp.API.Helpers;
using DatingApp.API.IRepo;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        Cloudinary Cloudinary;
        public PhotosController(IUnitOfWork unitOfWork, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.cloudinaryConfig = cloudinaryConfig;

            Account account = new Account
            (
                cloudinaryConfig.Value.CloudName,
                cloudinaryConfig.Value.ApiKey,
                cloudinaryConfig.Value.ApiSecret
            );
            Cloudinary = new Cloudinary(account);
        }
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {

            var photoFromRepo = await unitOfWork.PhotoRepository.GetById(id);

            var photo = mapper.Map<PhotoForReturn>(photoFromRepo);

            return Ok(photo);
        }
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreation photoForCreation)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await unitOfWork.UserRepository.GetById(userId);

            var file = photoForCreation.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = Cloudinary.Upload(uploadParams);
                }
            }
            photoForCreation.Url = uploadResult.Uri.ToString();
            photoForCreation.PublicId = uploadResult.PublicId;

            var newPhoto = mapper.Map<Photo>(photoForCreation);

            if (!userFromRepo.Photos.Any(p => p.IsMain))
                newPhoto.IsMain = true;

            userFromRepo.Photos.Add(newPhoto);

            if (await unitOfWork.SaveAll())
            {
                var photoToReturn = mapper.Map<PhotoForReturn>(newPhoto);
                return CreatedAtRoute("GetPhoto", new { id = newPhoto.Id }, photoToReturn);
            }

            throw new Exception($"Can't add image to user {userId}");

        }
        [HttpPost("{id}/setMain")]

        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await unitOfWork.UserRepository.GetById(userId);

            if (!userFromRepo.Photos.Any(p => p.Id == id))
                return BadRequest($"this photo {id} dosen't exist");

            var photoFromRepo = await unitOfWork.PhotoRepository.GetById(id);

            if (photoFromRepo.IsMain)
                return BadRequest($"this photo {id} is already the main");

            var mainPhoto = await unitOfWork.PhotoRepository.GetMainPhotoForUser(userId);

            mainPhoto.IsMain = false;
            photoFromRepo.IsMain = true;

            if (await unitOfWork.SaveAll())
                return NoContent();

            throw new Exception($"Can't set this photo as main");

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await unitOfWork.UserRepository.GetById(userId);

            if (!userFromRepo.Photos.Any(p => p.Id == id))
                return BadRequest($"this photo {id} dosen't exist");

            var photoFromRepo = await unitOfWork.PhotoRepository.GetById(id);
            if (photoFromRepo.IsMain)
                return BadRequest($"this photo {id} is already the main");

            var result = Cloudinary.Destroy(new DeletionParams(photoFromRepo.PublicId));
            if (result.Result == "ok")
                unitOfWork.PhotoRepository.Delete(photoFromRepo);

            if(photoFromRepo.PublicId==null)
                unitOfWork.PhotoRepository.Delete(photoFromRepo);

            if (await unitOfWork.SaveAll())
                return Ok();
            throw new Exception($"Couldn't delete user {id}");
        }
    }
}