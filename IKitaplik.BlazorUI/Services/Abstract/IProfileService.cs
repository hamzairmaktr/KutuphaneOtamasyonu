using IKitaplik.BlazorUI.Responses;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.UserDTOs;

namespace IKitaplik.BlazorUI.Services.Abstract
{
    public interface IProfileService
    {
        Task<Response<User>> GetProfile();
        Task<Response> UpdateProfile(UserProfileUpdateDto profileDto);
        Task<Response> ChangePassword(ChangePasswordDto passwordDto);
    }
}
