using Core.Utilities.Results;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.Entities.DTOs;

namespace IKitaplik.BlazorUI.Services.Abstract
{
    public interface IMovementService
    {
        Task<Response<PagedResult<MovementGetDTO>>> GetAllAsync(int page,int pageSize);
        Task<Response<PagedResult<MovementGetDTO>>> GetAllByBookIdAsync(int id, int page, int pageSize);
        Task<Response<PagedResult<MovementGetDTO>>> GetAllByBookNameAsync(string bookName, int page, int pageSize);
        Task<Response<PagedResult<MovementGetDTO>>> GetAllByDepositIdAsync(int id, int page, int pageSize);
        Task<Response<PagedResult<MovementGetDTO>>> GetAllByStudentIdAsync(int id, int page, int pageSize);
        Task<Response<PagedResult<MovementGetDTO>>> GetAllByStudentNameAsync(string studentName, int page, int pageSize);
        Task<Response<PagedResult<MovementGetDTO>>> GetAllByDonationIdAsync(int id, int page, int pageSize);
        Task<Response<MovementGetDTO>> GetByIdAsync(int id);
    }
}
