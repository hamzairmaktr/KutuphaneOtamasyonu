using Core.Utilities.Results;
using IKitaplik.BlazorUI.Responses;
using IKitaplik.Entities.DTOs.UserDTOs;

namespace IKitaplik.BlazorUI.Services.Abstract
{
    public interface IAuthService
    {
        Task<Response<LoginResponse>> Login(UserLoginDto userLoginDto);
        Task<Response> Register(UserRegisterDto userRegisterDto);
        Task<Response<LoginResponse>> RefresToken();
        Task<string> GetToken();
        Task LogOut();
    }
}
