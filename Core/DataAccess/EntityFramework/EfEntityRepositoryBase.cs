using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext
    {
        public readonly TContext _context;

        public EfEntityRepositoryBase(TContext context)
        {
            this._context = context;
        }

        public void Add(TEntity entity)
        {
            var addedEntity = _context.Entry(entity);
            addedEntity.State = EntityState.Added;
            _context.SaveChanges();
            addedEntity.State = EntityState.Detached;
        }

        public void Delete(TEntity entity)
        {
            if (entity is BaseEntities softDeletable)
            {
                var updatedEntity = _context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                softDeletable.IsDeleted = true;
                _context.SaveChanges();
                updatedEntity.State = EntityState.Detached;
            }
            else
            {
                var deletedEntity = _context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                _context.SaveChanges();
                deletedEntity.State = EntityState.Detached;
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null
                ? _context.Set<TEntity>().AsNoTracking().Where(p => EF.Property<bool>(p, "IsDeleted") == false).ToList()
                : _context.Set<TEntity>().AsNoTracking().Where(p => EF.Property<bool>(p, "IsDeleted") == false).Where(filter).ToList();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {

            return _context.Set<TEntity>().AsNoTracking().Where(p => EF.Property<bool>(p, "IsDeleted") == false).FirstOrDefault(filter);

        }

        public void Update(TEntity entity)
        {
            var updatedEntity = _context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            _context.SaveChanges();
            updatedEntity.State = EntityState.Detached;
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter)
        {
            return filter == null
               ? await _context.Set<TEntity>().AsNoTracking().Where(p => EF.Property<bool>(p, "IsDeleted") == false).ToListAsync()
               : await _context.Set<TEntity>().AsNoTracking().Where(p => EF.Property<bool>(p, "IsDeleted") == false).Where(filter).ToListAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _context.Set<TEntity>().AsNoTracking().Where(p => EF.Property<bool>(p, "IsDeleted") == false).FirstOrDefaultAsync(filter);
        }

        public async Task AddAsync(TEntity entity)
        {
            var addedEntity = _context.Entry(entity);
            addedEntity.State = EntityState.Added;
            await _context.SaveChangesAsync();
            addedEntity.State = EntityState.Detached;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            var updatedEntity = _context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            updatedEntity.State = EntityState.Detached;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (entity is BaseEntities softDeletable)
            {
                var updatedEntity = _context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                softDeletable.IsDeleted = true;
                await _context.SaveChangesAsync();
                updatedEntity.State = EntityState.Detached;
            }
            else
            {
                var deletedEntity = _context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                deletedEntity.State = EntityState.Detached;
            }
        }
    }
}
