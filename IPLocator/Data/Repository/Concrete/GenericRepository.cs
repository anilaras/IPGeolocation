using IPLocator.Data.Repository.Abstracts;
using IPLocator.Models.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IPLocator.Data.Repository.Concrete
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Table()
        {
            return Table<TEntity>();
        }
        public DbSet<TEntity> Table<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> metot)
        {
            return GetWhere<TEntity>(metot);
        }
        public IQueryable<A> GetWhere<A>(Expression<Func<A, bool>> metot) where A : class
        {
            return Table<A>().Where(metot);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public TEntity GetById(int id)
        {
            return _context.Set<TEntity>().AsNoTracking().FirstOrDefault(d => d.Id == id);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        }

        public void Create(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
        }

        public void BulkCreate(IQueryable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRangeAsync(entities);
            _context.SaveChanges();
        }
        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            _context.SaveChanges();
        }

        public void DeleteById(int id)
        {
            var entity = GetById(id);
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }


        public async Task CreateAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task BulkCreateAsync(IQueryable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
