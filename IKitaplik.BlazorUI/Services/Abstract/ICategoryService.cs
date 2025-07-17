using IKitaplik.BlazorUI.Responses;
using IKitaplik.Entities.DTOs.CategoryDTOs;

namespace IKitaplik.BlazorUI.Services.Abstract
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryGetDto>>> GetAllAsync();
        Task<Response<CategoryGetDto>> GetByIdAsync(int id);
        Task<Response> AddAsync(CategoryAddDto categoryAddDto);
        Task<Response> UpdateAsync(CategoryUpdateDto categoryUpdateDto);
        Task<Response> DeleteAsync(int id);
    }
}
