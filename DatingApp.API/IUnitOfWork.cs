using DatingApp.API.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API
{
    public interface IUnitOfWork
    {
        UserRepository UserRepository {get;}
        PhotoRepository PhotoRepository { get;}
        Task<bool> SaveAll();
    }
}
