using Core.Utilities.Results;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.DataAccess.UnitOfWork;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.BookDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IBookService
    {
        IDataResult<Book> Add(BookAddDto bookAddDto, bool isDonation = false);
        IResult Update(BookUpdateDto bookUpdateDto);
        IResult Delete(int id);
        IResult BookAddedPiece(BookAddPieceDto bookAddPieceDto, bool isDonationOrDeposit = false);

        IDataResult<List<BookGetDTO>> GetAllFiltered(BookFilterDto filter);
        IDataResult<List<BookGetDTO>> GetAll();
        IDataResult<List<BookGetDTO>> GetAllByName(string name);
        IDataResult<Book> GetById(int id);
        IDataResult<Book> GetByBarcode(string barcode);
    }
}
