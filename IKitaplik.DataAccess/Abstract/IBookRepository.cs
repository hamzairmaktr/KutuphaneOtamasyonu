using Core.DataAccess;
using IKitaplık.Entities.Concrete;
using IKitaplık.Entities.DTOs;
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
