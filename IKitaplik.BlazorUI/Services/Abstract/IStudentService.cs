using IKitaplik.BlazorUI.Responses;

namespace IKitaplik.BlazorUI.Services.Abstract
{
    public interface IStudentService
    {
        Task<Response<List<StudentGetDto>>> GetAllAsync();
        Task<Response<List<StudentGetDto>>> GetAllByNameAsync(string name);
        Task<Response<StudentGetDto>> GetByIdAsync(int id);
        Task<Response> AddAsync(StudentAddDto studentAddDto);
        Task<Response> UpdateAsync(StudentUpdateDto studentUpdateDto);
        Task<Response> DeleteAsync(int id);
    }
}