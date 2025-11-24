using Core.Contexts;
using Core.DataAccess.EntityFramework;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.BookDTOs;
using Microsoft.EntityFrameworkCore;
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
        public EfBookRepository(Context context,IUserContext context1) : base(context,context1)
        {
            this._context = context;
        }

        public List<BookGetDTO> GetAllBookDTOs(Expression<Func<BookGetDTO, bool>> filter = null)
        {
            var result = from b in _context.Books
                         join c in _context.Categories
                         on b.CategoryId equals c.Id
                         join w in _context.Writers
                         on b.WriterId equals w.Id
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
                             WriterName = w.WriterName,
                             PageSize = b.PageSize,
                             CreatedDate = b.CreatedDate,
                             UpdatedDate = b.UpdatedDate
                         };
            if (filter != null)
                result = result.Where(filter);
            return result.ToList();
        }

        public async Task<List<BookGetDTO>> GetAllBookDTOsAsync(Expression<Func<BookGetDTO, bool>> filter = null)
        {
            var result = from b in _context.Books
                         join c in _context.Categories
                         on b.CategoryId equals c.Id
                         join w in _context.Writers
                         on b.WriterId equals w.Id
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
                             WriterName = w.WriterName,
                             PageSize = b.PageSize,
                             CreatedDate = b.CreatedDate,
                             UpdatedDate = b.UpdatedDate
                         };
            if (filter != null)
                result = result.Where(filter);
            return await result.ToListAsync();
        }
    }
}
