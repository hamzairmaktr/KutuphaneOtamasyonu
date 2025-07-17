using IKitaplik.BlazorUI.Responses;
using IKitaplik.Entities.DTOs;

namespace IKitaplik.BlazorUI.Services.Abstract
{
    public interface IMovementService
    {
        Task<Response<List<MovementGetDTO>>> GetAllAsync();
        Task<Response<List<MovementGetDTO>>> GetAllByBookIdAsync(int id);
        Task<Response<List<MovementGetDTO>>> GetAllByBookNameAsync(string bookName);
        Task<Response<List<MovementGetDTO>>> GetAllByDepositIdAsync(int id);
        Task<Response<List<MovementGetDTO>>> GetAllByStudentIdAsync(int id);
        Task<Response<List<MovementGetDTO>>> GetAllByStudentNameAsync(string studentName);
        Task<Response<List<MovementGetDTO>>> GetAllByDonationIdAsync(int id);
        Task<Response<MovementGetDTO>> GetByIdAsync(int id);
    }
}
