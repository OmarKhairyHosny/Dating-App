using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public Seed(UserManager<User> userManager,RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public void SeedUser()
        {
            if (!userManager.Users.Any())
            {
                var usersData = File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(usersData);
                var roles = new List<Role>
                {
                    new Role{Name="Member"},
                    new Role{Name="Admin"},
                    new Role{Name="Moderator"},
                    new Role{Name="VIP"}
                };

                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    user.SecurityStamp = DateTime.Now.Ticks.ToString();
                    user.Photos.SingleOrDefault().IsApproved = true;
                    userManager.CreateAsync(user, "a1234").Wait();
                    userManager.AddToRoleAsync(user, "Member").Wait();
                }
                var adminUser = new User
                {
                    UserName = "Admin"
                };
              //  var result = await userManager.CreateAsync(adminUser, "1234");

                /// anothor way user Result instead of await !!
                var result = userManager.CreateAsync(adminUser, "a1234").Result;

                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" }).Wait();
                    
                }
            }
         
        }
       
    }
}
