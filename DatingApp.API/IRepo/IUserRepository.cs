using DatingApp.API.Helpers;
using DatingApp.API.IRepo;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.IRepo
{
    public interface IUserRepository:IRepository<User>
    {
        Task<bool> SaveAll();
        Task<PagedList<User>> GetAll(UserParams userParams);
        Task<Like> GetLike(int userId, int recipientId);
        Task<User> GetById(int id, bool isCurrentUser);


        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId);
    }
}
