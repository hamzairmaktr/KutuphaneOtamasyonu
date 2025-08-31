using Core.Utilities.Results;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.BookDTOs;

namespace IKitaplik.BlazorUI.Services.Abstract
{
    public interface IBookService
    {
        Task<Response<List<BookGetDTO>>> GetAllBooksAsync();
        Task<Response<List<BookGetDTO>>> GetAllActiveBooksAsync();
        Task<Response<Book>> GetBookByIdAsync(int id);
        Task<Response> AddBookAsync(BookAddDto dto);
        Task<Response> UpdateBookAsync(BookUpdateDto dto);
        Task<Response> BookAddPieceAsync(BookAddPieceDto dto);
        Task<Response> DeleteBookAsync(int id);
    }
}
