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
        Task<IDataResult<Book>> AddAsync(BookAddDto bookAddDto, bool isDonation = false);
        Task<IResult> UpdateAsync(BookUpdateDto bookUpdateDto);
        Task<IResult> DeleteAsync(int id);
        Task<IResult> BookAddedPieceAsync(BookAddPieceDto bookAddPieceDto, bool isDonationOrDeposit = false);

        Task<IDataResult<PagedResult<BookGetDTO>>> GetAllFilteredAsync(int page, int pageSize, BookFilterDto filter);
        Task<IDataResult<PagedResult<BookGetDTO>>> GetAllAsync(int page, int pageSize);
        Task<IDataResult<PagedResult<BookGetDTO>>> GetAllActiveAsync(int page, int pageSize);
        Task<IDataResult<List<BookGetDTO>>> GetAllByNameAsync(string name);
        Task<IDataResult<Book>> GetByIdAsync(int id);
        Task<IDataResult<Book>> GetByBarcodeAsync(string barcode);
    }
}
