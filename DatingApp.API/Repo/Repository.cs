using DatingApp.API.IRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.API.Repo
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class 
    {
        DbContext ctx;
        DbSet<TEntity> set;
        public Repository(DbContext ctx)
        {
            this.ctx = ctx;
            set = ctx.Set<TEntity>();
        }
        public T Add<T>(T entity) where T : class
        {
            ctx.Set<T>().Add(entity);
            return entity;
        }
        public TEntity Add(TEntity entity)
        {
           
            set.Add(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
           set.Remove(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            ctx.Set<T>().Remove(entity);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            return await set.ToListAsync();
        } 
        public virtual async Task<TEntity> GetById(int id)
        {
            return await set.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            set.Attach(entity);
            ctx.Entry(entity).State = EntityState.Modified;
        }
 
    }
}
