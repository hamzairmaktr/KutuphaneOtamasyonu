using Core.DataAccess.EntityFramework;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
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
        public EfMovomentRepository(Context context) : base(context)
        {
            _context = context;
        }

        public List<MovementGetDTO> GetAllDTO(Expression<Func<MovementGetDTO, bool>> filter = null)
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
                ? result.ToList()
                : result.Where(filter).ToList();
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
    }
}
