using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Data;
using DatingApp.API.Models;
using DatingApp.API.IRepo;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Dtos.User;
using System.Security.Claims;
using DatingApp.API.Helpers;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository repo;
        private readonly IMapper mapper;

        public UsersController(IUserRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId =int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var currentUser = await repo.GetById(currentUserId);

            userParams.UserId = currentUserId;
            if (String.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = currentUser.Gender == "male" ? "female" : "male";
            }
            var users = await repo.GetAll(userParams);
            var usersToReturn = mapper.Map<IEnumerable<UserForList>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(usersToReturn);
        }

        // GET: api/Users/5
        [HttpGet("{id}",Name ="GetUser")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var isCurrentUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) == id;
            var user = await repo.GetById(id,isCurrentUser);

            var userToReturn = mapper.Map<UserForDetail>(user);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(userToReturn);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UserForUpdate user)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var UserFromRepo = await repo.GetById(id);

            mapper.Map(user, UserFromRepo);
            if (await repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating User {id} failed");

        }

        [HttpPost("{id}/like/{recipientId}")]

        public async Task<IActionResult> LikeUser(int id,int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var UserFromRepo = await repo.GetById(id);

            var like = await repo.GetLike(id, recipientId);

            if (like != null)
                return BadRequest("you already like this match");

            if (await repo.GetById(recipientId) == null)
                return NotFound();

            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            repo.Add<Like>(like);

            if (await repo.SaveAll())
                return Ok();

            return BadRequest("Failed to like user");

        }


        //// DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var UserFromRepo = await repo.GetById(id);

            repo.Delete(UserFromRepo);

            if (await repo.SaveAll())
                return Ok(UserFromRepo);
            throw new Exception($"Couldn't delete user {id}");
        }

        //private bool UserExists(int id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}