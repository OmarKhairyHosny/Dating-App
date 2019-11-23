using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.IRepo
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        Task<bool> SaveAll();
        Task<Photo> GetMainPhotoForUser(int userId);
    }
}
