using Core.Utilities.Results;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.Entities.DTOs.StudentDTOs;

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
        Task<Response<List<StudentAutocompleteDto>>> SearchStudentsAsync(string query);
        Task<Response<StudentGetDto>> GetStudentDtoByIdAsync(int id);
    }
}