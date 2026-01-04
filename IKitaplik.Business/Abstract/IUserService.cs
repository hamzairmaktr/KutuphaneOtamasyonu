using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Abstract
{
    public interface IUserService
    {
        Task<IResult> RegisterAsync(UserRegisterDto userRegisterDto);
        Task<IDataResult<User>> LoginAsync(UserLoginDto userLoginDto);
        Task<IDataResult<User>> GetByRefreshTokenAsync(string refreshToken);
        Task<IResult> SetRefreshTokenAsync(string refreshToken, DateTime refreshTokenExpiryTime, int id);

        // Security
        Task<IDataResult<User>> VerifyTwoFactorAsync(VerifyTwoFactorDto verifyTwoFactorDto);
        Task<IResult> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<IResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<IResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<IResult> UpdateProfileAsync(UserProfileUpdateDto userProfileUpdateDto);
        Task<IDataResult<User>> GetByIdAsync(int id);
    }
}
