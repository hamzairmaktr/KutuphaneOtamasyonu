using Core.Utilities.Results;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.Entities.DTOs.WriterDTOs;

namespace IKitaplik.BlazorUI.Services.Abstract
{
    public interface IWriterService
    {
        Task<Response<PagedResult<WriterGetDto>>> GetAllAsync(int page, int pageSize);
        Task<Response<WriterGetDto>> GetAsync(int id);
        Task<Response> AddAsync(WriterAddDto writerAddDto);
        Task<Response> DeleteAsync(int id);
        Task<Response> UpdateAsync(WriterUpdateDto writerUpdateDto);
    }
}
