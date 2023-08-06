using IPLocator.Models.Concrete;
using System.Linq.Expressions;

namespace IPLocator.Data.Repository.Abstracts
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {

        IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> metot);
        IQueryable<A> GetWhere<A>(Expression<Func<A, bool>> metot) where A : class;
        IQueryable<TEntity> GetAll();
        TEntity GetById(int id);
        void Create(TEntity entity);
        void BulkCreate(IQueryable<TEntity> entity);
        void Update(TEntity entity);
        void DeleteById(int id);
        void Delete(TEntity entity);
        Task<TEntity> GetByIdAsync(int id);
        Task CreateAsync(TEntity entity);
        Task BulkCreateAsync(IQueryable<TEntity> entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteByIdAsync(int id);
        Task DeleteAsync(TEntity entity);
    }
}
