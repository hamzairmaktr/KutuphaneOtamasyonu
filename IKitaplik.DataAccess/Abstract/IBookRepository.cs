using Core.DataAccess;
using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.BookDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.DataAccess.Abstract
{
    public interface IBookRepository : IEntityRepository<Book>
    {
        PagedResult<BookGetDTO> GetAllBookDTOs(int page, int pageSize, Expression<Func<BookGetDTO, bool>>? filter = null);
        Task<PagedResult<BookGetDTO>> GetAllBookDTOsAsync(int page, int pageSize, Expression<Func<BookGetDTO, bool>>? filter = null);
    }
}
