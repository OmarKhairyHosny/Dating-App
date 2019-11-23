using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.IRepo;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration config;
        private readonly IMapper mapper;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
            this.mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegister user)
        {

            var userToCreate = mapper.Map<User>(user);

            var result = await userManager.CreateAsync(userToCreate, user.Password);

            var userToReturn = mapper.Map<UserForDetail>(userToCreate);

            if (result.Succeeded)
                return CreatedAtRoute("GetUser", new { controller = "Users", id = userToCreate.Id }, userToReturn);

            return BadRequest(result.Errors);


        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLogin user)
        {
            var userFromManager = await userManager.FindByNameAsync(user.UserName);

            var result = await signInManager.CheckPasswordSignInAsync(userFromManager, user.Password, false);

            if (result.Succeeded)
            {
                var userExist = await userManager.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.NormalizedUserName == user.UserName.ToUpper());

                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.NameIdentifier,userExist.Id.ToString()),
                new Claim(ClaimTypes.Name,userExist.UserName)
            };

                var roles = await userManager.GetRolesAsync(userExist);

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var x = config.GetSection("Token").Value;
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Token").Value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(descriptor);

                var userToReturn = mapper.Map<UserForList>(userExist);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token),
                    user = userToReturn
                });
            }
            return Unauthorized();

        }

    }
}