using Core.DataAccess;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.DataAccess.Abstract
{
    public interface IBookRepository:IEntityRepository<Book>
    {
        List<BookGetDTO> GetAllBookDTOs();
        List<BookGetDTO> GetAllBookFilteredDTOs(Expression<Func<BookGetDTO, bool>> filter);
    }
}
