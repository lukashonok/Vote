using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;

namespace Repositories
{
    public interface IRepository<T>
    {
        T Get(int id);
        IQueryable<T> GetAll();
        EntityEntry Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Remove(T entity);
        void SaveChanges();
    }
}
