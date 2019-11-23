using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos.User;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DatingAppContext context;
        private readonly UserManager<User> userManager;

        public AdminController(DatingAppContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [Authorize(Policy = "RequiredAdminRole")]
        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var users = await (from user in context.Users
                               orderby user.UserName
                               select new
                               {
                                   Id = user.Id,
                                   UserName = user.UserName,
                                   Roles = (from userRole in user.UserRoles
                                            join role in context.Roles
                                            on userRole.RoleId
                                            equals role.Id
                                            select role.Name).ToList()
                               }
                              ).ToListAsync();

            return Ok(users);
        }
        [Authorize(Policy = "RequiredAdminRole")]
        [HttpPost("editRoles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName,RoleEdit roles)
        {
            var user =await userManager.FindByNameAsync(userName);
            var userRoles = await userManager.GetRolesAsync(user);
            var selectedRoles = roles.RolesNames;

            selectedRoles = selectedRoles ?? new string[] { };

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to add new roles");


            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to remove user roles");

            return Ok(await userManager.GetRolesAsync(user));
               
        }
      
       
    }
}