using Core.Utilities.Results;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.Entities.DTOs.BookDTOs;

namespace IKitaplik.BlazorUI.Services.Abstract
{
    public interface IBookService
    {
        Task<Response<List<BookGetDTO>>> GetAllBooksAsync();
        Task<Response<BookGetDTO>> GetBookDetailsAsync(int id);
        Task<Response> AddBookAsync<T>(BookAddDto dto);
        Task<Response> UpdateBookAsync(BookUpdateDto dto);
        Task<Response> DeleteBookAsync(int id);
    }
}
