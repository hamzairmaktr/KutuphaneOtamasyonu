using Core.Contexts;
using Core.DataAccess.EntityFramework;
using Core.Utilities.Results;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.DataAccess.Concrete.EntityFramework
{
    public class EfMovomentRepository : EfEntityRepositoryBase<Movement, Context>, IMovementRepository
    {
        Context _context;
        public EfMovomentRepository(Context context, IUserContext userContext) : base(context, userContext)
        {
            _context = context;
        }

        public PagedResult<MovementGetDTO> GetAllDTO(int page, int pageSize, Expression<Func<MovementGetDTO, bool>> filter = null)
        {
            var result = from movement in _context.Movements
                         join book in _context.Books
                         on movement.BookId equals book.Id
                         join student in _context.Students
                         on movement.StudentId equals student.Id
                         where movement.IsDeleted == false
                         select new MovementGetDTO
                         {
                             Id = movement.Id,
                             BookName = book.Name,
                             MovementDate = movement.MovementDate,
                             Note = movement.Note,
                             StudentName = student.Name,
                             Title = movement.Title,
                             BookId = book.Id,
                             DepositId = movement.DepositId,
                             DonationId = movement.DonationId,
                             StudentId = movement.StudentId,
                             Type = movement.Type
                         };
            if (filter != null)
                result = result.Where(filter);
            var totalCount = result.Count();
            var items = result.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return new PagedResult<MovementGetDTO>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<PagedResult<MovementGetDTO>> GetAllDTOAsync(int page, int pageSize, Expression<Func<MovementGetDTO, bool>> filter = null)
        {
            var result = from movement in _context.Movements
                         join book in _context.Books
                         on movement.BookId equals book.Id
                         join student in _context.Students
                         on movement.StudentId equals student.Id
                         where movement.IsDeleted == false
                         select new MovementGetDTO
                         {
                             Id = movement.Id,
                             BookName = book.Name,
                             MovementDate = movement.MovementDate,
                             Note = movement.Note,
                             StudentName = student.Name,
                             Title = movement.Title,
                             BookId = book.Id,
                             DepositId = movement.DepositId,
                             DonationId = movement.DonationId,
                             StudentId = movement.StudentId,
                             Type = movement.Type
                         };
            if (filter != null)
                result = result.Where(filter);
            var totalCount = await result.CountAsync();
            var items = await result.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedResult<MovementGetDTO>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public MovementGetDTO GetDTO(Expression<Func<MovementGetDTO, bool>> filter)
        {
            var result = from movement in _context.Movements
                         join book in _context.Books
                         on movement.BookId equals book.Id
                         join student in _context.Students
                         on movement.StudentId equals student.Id
                         where movement.IsDeleted == false
                         select new MovementGetDTO
                         {
                             Id = movement.Id,
                             BookName = book.Name,
                             MovementDate = movement.MovementDate,
                             Note = movement.Note,
                             StudentName = student.Name,
                             Title = movement.Title,
                             BookId = book.Id,
                             DepositId = movement.DepositId,
                             DonationId = movement.DonationId,
                             StudentId = movement.StudentId,
                             Type = movement.Type
                         };
            return filter == null
                ? result.SingleOrDefault()
                : result.Where(filter).SingleOrDefault();
        }

        public async Task<MovementGetDTO> GetDTOAsync(Expression<Func<MovementGetDTO, bool>> filter)
        {
            var result = from movement in _context.Movements
                         join book in _context.Books
                         on movement.BookId equals book.Id
                         join student in _context.Students
                         on movement.StudentId equals student.Id
                         where movement.IsDeleted == false
                         select new MovementGetDTO
                         {
                             Id = movement.Id,
                             BookName = book.Name,
                             MovementDate = movement.MovementDate,
                             Note = movement.Note,
                             StudentName = student.Name,
                             Title = movement.Title,
                             BookId = book.Id,
                             DepositId = movement.DepositId,
                             DonationId = movement.DonationId,
                             StudentId = movement.StudentId,
                             Type = movement.Type
                         };
            return filter == null
                ? await result.SingleOrDefaultAsync()
                : await result.Where(filter).SingleOrDefaultAsync();
        }
    }
}
