using Core.DataAccess.EntityFramework;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.BookDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.DataAccess.Concrete.EntityFramework
{
    public class EfBookRepository : EfEntityRepositoryBase<Book, Context>, IBookRepository
    {
        private readonly Context _context;
        public EfBookRepository(Context context) : base(context)
        {
            this._context = context;
        }

        public List<BookGetDTO> GetAllBookDTOs()
        {
            var result = from b in _context.Books
                         join c in _context.Categories
                         on b.CategoryId equals c.Id
                         where b.IsDeleted == false
                         select new BookGetDTO
                         {
                             Barcode = b.Barcode,
                             CategoryName = c.Name,
                             Name = b.Name,
                             Piece = b.Piece,
                             Id = b.Id,
                             ShelfNo = b.ShelfNo,
                             Situation = b.Situation,
                             Writer = b.Writer,
                             PageSize = b.PageSize,
                             CreatedDate = b.CreatedDate,
                             UpdatedDate = b.UpdatedDate
                         };
            return result.ToList();
        }

        public List<BookGetDTO> GetAllBookFilteredDTOs(Expression<Func<BookGetDTO, bool>> filter)
        {
            var result = from b in _context.Books
                         join c in _context.Categories
                         on b.CategoryId equals c.Id
                         where b.IsDeleted == false
                         select new BookGetDTO
                         {
                             Barcode = b.Barcode,
                             CategoryName = c.Name,
                             Name = b.Name,
                             Piece = b.Piece,
                             Id = b.Id,
                             ShelfNo = b.ShelfNo,
                             Situation = b.Situation,
                             Writer = b.Writer,
                             PageSize = b.PageSize,
                             CreatedDate = b.CreatedDate,
                             UpdatedDate = b.UpdatedDate
                         };
            return result.Where(filter).ToList();
        }
    }
}
