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
using Core.Contexts;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext
    {
        public readonly TContext _context;
        private readonly IUserContext _userContext;
        public EfEntityRepositoryBase(TContext context, IUserContext userContext)
        {
            this._context = context;
            _userContext = userContext;
        }

        public void Add(TEntity entity)
        {
            if (entity is BaseEntities baseEntity)
            {
                var addedEntity = _context.Entry(entity);
                addedEntity.State = EntityState.Added;
                baseEntity.UserId = int.Parse(_userContext.UserId);
                _context.SaveChanges();
                addedEntity.State = EntityState.Detached;
            }
            else
            {
                var addedEntity = _context.Entry(entity);
                addedEntity.State = EntityState.Added;
                _context.SaveChanges();
                addedEntity.State = EntityState.Detached;
            }
        }

        public void Delete(TEntity entity)
        {
          
            if (entity is IEntity softDeletable)
            {
                var updatedEntity = _context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                softDeletable.IsDeleted = true;
                _context.SaveChanges();
                updatedEntity.State = EntityState.Detached;
            }

        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null
                ? _context.Set<TEntity>().AsNoTracking().ToList()
                : _context.Set<TEntity>().AsNoTracking().Where(filter).ToList();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {

            return _context.Set<TEntity>().AsNoTracking().FirstOrDefault(filter);

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
               ? await _context.Set<TEntity>().AsNoTracking().ToListAsync()
               : await _context.Set<TEntity>().AsNoTracking().Where(filter).ToListAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filter);
        }

        public async Task AddAsync(TEntity entity)
        {
            if (entity is BaseEntities baseEntity)
            {
                baseEntity.UserId = int.Parse(_userContext.UserId);
            }
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
            if (entity is IEntity softDeletable)
            {
                var updatedEntity = _context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                softDeletable.IsDeleted = true;
                await _context.SaveChangesAsync();
                updatedEntity.State = EntityState.Detached;
            }
        }

        public void DeleteRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
                Delete(entity);
        }

        public async Task DeleteRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
                await DeleteAsync(entity);
        }

        public void AddRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is BaseEntities baseEntity)
                {
                    baseEntity.UserId = int.Parse(_userContext.UserId);
                }
            }
            _context.AddRange(entities);
            _context.SaveChanges();
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity is BaseEntities baseEntity)
                {
                    baseEntity.UserId = int.Parse(_userContext.UserId);
                }
            }
            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }
    }
}
