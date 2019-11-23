using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Repo;

namespace DatingApp.API
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatingAppContext ctx;


        public UnitOfWork(DatingAppContext ctx)
        {
            this.ctx = ctx;
        }
        private UserRepository userRepository;
        public UserRepository UserRepository
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(ctx);
                return userRepository;
            }
        }
        private PhotoRepository photoRepository;
        public PhotoRepository PhotoRepository
        {
            get
            {
                if (photoRepository == null)
                    photoRepository = new PhotoRepository(ctx);
                return photoRepository;
            }
        }
        public async Task<bool> SaveAll()
        {
            return await ctx.SaveChangesAsync() > 0;
        }
    }
}
