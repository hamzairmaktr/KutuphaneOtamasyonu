using Core.Utilities.Results;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.Entities.DTOs.BookDTOs;
using IKitaplik.Entities.DTOs.DonationDTOs;

namespace IKitaplik.BlazorUI.Services.Abstract
{
    public interface IDonationService
    {
        Task<Response> AddAsync(DonationAddDto donationAddDto);
        Task<Response<PagedResult<DonationGetDTO>>> GetAllAsync(int page,int pageSize);
        Task<Response<DonationGetDTO>> GetByIdAsync(int id);
    }
}
