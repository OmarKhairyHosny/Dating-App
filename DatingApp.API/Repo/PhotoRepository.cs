using DatingApp.API.Data;
using DatingApp.API.IRepo;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Repo
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        private readonly DatingAppContext ctx;

        public PhotoRepository(DatingAppContext ctx) : base(ctx)
        {
            this.ctx = ctx;
        }

        public override async Task<Photo> GetById(int id)
        {
            var photo = await ctx.Photos.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            var photo = await ctx.Photos.Where(p => p.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
            return photo;
        }

        public async Task<bool> SaveAll()
        {
            return await ctx.SaveChangesAsync() > 0;
        }
    }
}
