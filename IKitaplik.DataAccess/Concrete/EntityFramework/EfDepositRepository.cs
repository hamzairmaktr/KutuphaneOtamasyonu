using Core.Contexts;
using Core.DataAccess.EntityFramework;
using Core.Utilities.Results;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.DepositDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.DataAccess.Concrete.EntityFramework
{
    public class EfDepositRepository : EfEntityRepositoryBase<Deposit, Context>, IDepositRepository
    {
        private readonly Context _context;
        public EfDepositRepository(Context context,IUserContext userContext) : base(context,userContext)
        {
            this._context = context;
        }

        public PagedResult<DepositGetDTO> GetAllDepositDTOs(int page, int pageSize, Expression<Func<DepositGetDTO, bool>> filter = null)
        {
            var result = from d in _context.Deposits
                         join b in _context.Books
                         on d.BookId equals b.Id
                         join s in _context.Students
                         on d.StudentId equals s.Id
                         where d.IsDeleted == false
                         select new DepositGetDTO
                         {
                             AmILate = d.AmILate,
                             BookName = b.Name,
                             DeliveryDate = d.DeliveryDate,
                             Id = d.Id,
                             IsItDamaged = d.IsItDamaged,
                             IssueDate = d.IssueDate,
                             Note = d.Note,
                             StudentName = s.Name,
                             IsDelivered = d.IsDelivered
                         };
            result = filter == null ? result : result.Where(filter);
            var totalCount = result.Count();
            var items = result.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResult<DepositGetDTO>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<DepositGetDTO>> GetAllDepositDTOsAsync(int page, int pageSize, Expression<Func<DepositGetDTO, bool>> filter = null)
        {
            var result = from d in _context.Deposits
                         join b in _context.Books
                         on d.BookId equals b.Id
                         join s in _context.Students
                         on d.StudentId equals s.Id
                         where d.IsDeleted == false
                         select new DepositGetDTO
                         {
                             AmILate = d.AmILate,
                             BookName = b.Name,
                             DeliveryDate = d.DeliveryDate,
                             Id = d.Id,
                             IsItDamaged = d.IsItDamaged,
                             IssueDate = d.IssueDate,
                             Note = d.Note,
                             StudentName = s.Name,
                             IsDelivered = d.IsDelivered
                         };
            if (filter != null)
                result = result.Where(filter);
            var totalCount = await result.CountAsync();
            var items = await result.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedResult<DepositGetDTO>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public DepositGetDTO GetDepositFilteredDTOs(Expression<Func<DepositGetDTO, bool>> filter)
        {
            var result = from d in _context.Deposits
                         join b in _context.Books
                         on d.BookId equals b.Id
                         join s in _context.Students
                         on d.StudentId equals s.Id
                         where d.IsDeleted == false
                         select new DepositGetDTO
                         {
                             AmILate = d.AmILate,
                             BookName = b.Name,
                             DeliveryDate = d.DeliveryDate,
                             Id = d.Id,
                             IsItDamaged = d.IsItDamaged,
                             IssueDate = d.IssueDate,
                             Note = d.Note,
                             StudentName = s.Name,
                             IsDelivered = d.IsDelivered
                         };
            return result.Where(filter).FirstOrDefault();
        }

        public async Task<DepositGetDTO> GetDepositFilteredDTOsAsync(Expression<Func<DepositGetDTO, bool>> filter)
        {
            var result = from d in _context.Deposits
                         join b in _context.Books
                         on d.BookId equals b.Id
                         join s in _context.Students
                         on d.StudentId equals s.Id
                         where d.IsDeleted == false
                         select new DepositGetDTO
                         {
                             AmILate = d.AmILate,
                             BookName = b.Name,
                             DeliveryDate = d.DeliveryDate,
                             Id = d.Id,
                             IsItDamaged = d.IsItDamaged,
                             IssueDate = d.IssueDate,
                             Note = d.Note,
                             StudentName = s.Name,
                             IsDelivered = d.IsDelivered
                         };
            return await result.Where(filter).FirstOrDefaultAsync();
        }
    }
}
