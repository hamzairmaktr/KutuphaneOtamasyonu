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
using Core.Utilities.Results;

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

        public async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Expression<Func<TEntity, object>>? orderBy = null, bool desc = false)
        {
            var query = _context.Set<TEntity>().AsNoTracking().AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = desc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }
            else
            {
                query = query.OrderByDescending(e => EF.Property<object>(e, "Id"));
            }
            int count = query.Count();
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
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
        public async Task DeleteRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
                await DeleteAsync(entity);
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

        public async Task<PagedResult<TEntity>> GetAllAsyncPageResult(int page = 1, int pageSize = 20, Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>? orderBy = null, bool desc = false)
        {
            var query = _context.Set<TEntity>().AsNoTracking().AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = desc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }
            else
            {
                query = query.OrderByDescending(e => EF.Property<object>(e, "Id"));
            }
            int count = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var result = await query.ToListAsync();
            return new PagedResult<TEntity>
            {
                Items = result,
                TotalCount = count,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
