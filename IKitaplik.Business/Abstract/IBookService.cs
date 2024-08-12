using Core.Utilities.Results;
using IKitaplik.DataAccess.Abstract;
using IKitaplık.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IBookService
    {
        IResult Add(Book book);
        IResult Update(Book book);
        IResult Delete(Book book);

        IDataResult<List<Book>> GetAll();
        IDataResult<Book> GetById(int id);
        IDataResult<Book> GetByName(string name);
    }
}
