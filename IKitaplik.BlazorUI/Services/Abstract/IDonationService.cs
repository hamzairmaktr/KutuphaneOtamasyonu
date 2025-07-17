using IKitaplik.BlazorUI.Responses;
using IKitaplik.Entities.DTOs.BookDTOs;
using IKitaplik.Entities.DTOs.DonationDTOs;

namespace IKitaplik.BlazorUI.Services.Abstract
{
    public interface IDonationService
    {
        Task<Response> AddAsync(DonationAddDto donationAddDto);
        Task<Response<List<DonationGetDTO>>> GetAllAsync();
        Task<Response<DonationGetDTO>> GetByIdAsync(int id);
    }
}
