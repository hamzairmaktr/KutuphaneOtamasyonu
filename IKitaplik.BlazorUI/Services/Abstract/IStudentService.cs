using Core.Utilities.Results;
using IKitaplik.BlazorUI.Responses;

namespace IKitaplik.BlazorUI.Services.Abstract
{
    public interface IStudentService
    {
        Task<Response<PagedResult<StudentGetDto>>> GetAllAsync(int page, int pageSize);
        Task<Response<PagedResult<StudentGetDto>>> GetAllActiveAsync(int page, int pageSize);
        Task<Response<List<StudentGetDto>>> GetAllByNameAsync(string name);
        Task<Response<StudentGetDto>> GetByIdAsync(int id);
        Task<Response> AddAsync(StudentAddDto studentAddDto);
        Task<Response> UpdateAsync(StudentUpdateDto studentUpdateDto);
        Task<Response> DeleteAsync(int id);
    }
}