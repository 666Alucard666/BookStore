using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace DAL.Abstractions.Interfaces
{
    public interface IGenericRepos<TEntity> where TEntity:BaseEntity
    {
        void Create(TEntity item);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault();
        Task<TEntity> FirstOrDefaultAsync();
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity FindById(int id);
        IQueryable<TEntity> GetAll();
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
        Task<bool> Any();
        void Update(TEntity item);
        void RemoveById(int id);
        void Remove(TEntity item);
        void Save();
    }
}
