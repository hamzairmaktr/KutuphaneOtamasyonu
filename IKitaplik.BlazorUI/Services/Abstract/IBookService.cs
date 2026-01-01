using Core.Utilities.Results;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.BookDTOs;

namespace IKitaplik.BlazorUI.Services.Abstract
{
    public interface IBookService
    {
        Task<Response<PagedResult<BookGetDTO>>> GetAllBooksAsync(int page, int pageSize);
        Task<Response<PagedResult<BookGetDTO>>> GetAllActiveBooksAsync(int page, int pageSize);
        Task<Response<Book>> GetBookByIdAsync(int id);
        Task<Response<Book>> GetBookByBarcodeAsync(string barcode);
        Task<Response> AddBookAsync(BookAddDto dto);
        Task<Response> UpdateBookAsync(BookUpdateDto dto);
        Task<Response> BookAddPieceAsync(BookAddPieceDto dto);
        Task<Response> DeleteBookAsync(int id);
        Task<Response<List<BookAutocompleteDto>>> SearchBooksAsync(string query);
        Task<Response<BookGetDTO>> GetBookDtoByIdAsync(int id);
    }
}
