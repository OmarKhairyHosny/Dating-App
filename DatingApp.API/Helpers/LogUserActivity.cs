using DatingApp.API.IRepo;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
namespace DatingApp.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var result = await next();
            var userId = int.Parse(result.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var repo = result.HttpContext.RequestServices.GetService<IUserRepository>();

            var user = await repo.GetById(userId);

            user.LastActive = DateTime.Now;

            await repo.SaveAll();
        }
    }
}
