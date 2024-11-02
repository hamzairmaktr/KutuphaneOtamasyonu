using Core.Utilities.Results;
using IKitaplik.DataAccess.Abstract;
using IKitaplık.Entities.Concrete;
using IKitaplık.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IBookService
    {
        IDataResult<Book> Add(Book book);
        IResult Update(Book book);
        IResult Delete(int id);
        IResult BookAddedPiece(Book books);

        IDataResult<List<BookGetDTO>> GetAll();
        IDataResult<List<BookGetDTO>> GetAllByName(string name);
        IDataResult<Book> GetById(int id);
        IDataResult<Book> GetByBarcode(string barcode);
    }
}
