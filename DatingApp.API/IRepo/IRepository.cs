using DatingApp.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.API.IRepo
{
    public interface IRepository<TEntity> where TEntity:class
    {
        TEntity Add(TEntity entity);
        T Add<T>(T entity) where T : class;
        Task<IEnumerable<TEntity>> GetAll(); 
        Task<TEntity> GetById(int id);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete<T>(T entity) where T : class;
    }
}
