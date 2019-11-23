using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.IRepo;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Repo
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly DatingAppContext context;

        public UserRepository(DatingAppContext ctx) : base(ctx)
        {
            this.context = ctx;
        }
        public override async Task<IEnumerable<User>> GetAll()
        {
            var users = await context.Users.Include(p => p.Photos).ToListAsync();

            return users;
        }
        public async Task<PagedList<User>> GetAll(UserParams userParams)
        {
            var items = context.Users.Include(p => p.Photos).OrderByDescending(x => x.LastActive).AsQueryable();
            items = items.Where(u => u.Id != userParams.UserId);
            items = items.Where(u => u.Gender == userParams.Gender);

            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-userParams.MaxAge);
                var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

                items = items.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                items = items.Where(u => userLikers.Contains(u.Id));
            }

            if (userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                items = items.Where(u => userLikees.Contains(u.Id));
            }
            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        items = items.OrderByDescending(u => u.Created);
                        break;
                    default:
                        items = items.OrderByDescending(u => u.LastActive);
                        break;
                }
            }
            var users = await PagedList<User>.CreateAsync(items, userParams.PageNumber, userParams.PageSize);

            return users;
        }
        public async Task<User> GetById(int id,bool isCurrentUser)
        {
            var query = context.Users.Include(p => p.Photos).AsQueryable();

            if (isCurrentUser)
                query = query.IgnoreQueryFilters();

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<Like> GetLike(int userId, int recipientId)
        {
            return await context.Likes.FirstOrDefaultAsync(l => l.LikerId == userId && l.LikeeId == recipientId);
        }
        private async Task<IEnumerable<int>> GetUserLikes(int userId, bool liker)
        {
            var user = await context.Users.Include(x => x.Likees).Include(x => x.Likers).FirstOrDefaultAsync(
                u => u.Id == userId);
            if (liker)
                return user.Likers.Select(u => u.LikerId);
            else
                return user.Likees.Select(u => u.LikeeId);
        }

        public async Task<bool> SaveAll()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await context.Message.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            return await context.Message
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos).
                Where(m => m.SenderId == userId && m.RecipientId == recipientId && m.SenderDeleted == false
                             || m.SenderId==recipientId&&m.RecipientId==userId && m.RecipientDeleted == false).ToListAsync();
         }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
            var messages = context.Message
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                .AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId
                        && u.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId
                        && u.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId
                        && u.RecipientDeleted == false && u.IsRead == false);
                    break;
            }
            messages = messages.OrderByDescending(d => d.MessageSent);

            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);

        }
    }
} 
