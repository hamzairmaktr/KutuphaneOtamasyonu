using Core.Entities;
using Core.Utilities.Results;
using IKitaplik.Business.Abstract;
using IKitaplik.Business.Helpers;
using IKitaplik.Business.Validations.FluentValidations;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.DataAccess.UnitOfWork;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IKitaplik.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserValidator _userValidator;
        private readonly Core.Utilities.Security.Email.IEmailService _emailService;

        public UserManager(IUnitOfWork unitOfWork, UserValidator userValidator, Core.Utilities.Security.Email.IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _userValidator = userValidator;
            _emailService = emailService;
        }

        public async Task<IDataResult<User>> GetByRefreshTokenAsync(string refreshToken)
        {
            var user = await _unitOfWork.Users.GetAsync(p => p.RefreshToken == refreshToken && p.RefreshTokenExpiryTime > DateTime.Now);
            if (user == null)
                return new ErrorDataResult<User>("Refresh tokenın süresi doldu login olunuz");
            return new SuccessDataResult<User>(user);
        }

        public async Task<IDataResult<User>> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Username == userLoginDto.Username);

            if (user == null || !PasswordHasher.Verify(userLoginDto.Password, user.PasswordHash))
                return new ErrorDataResult<User>("Hatalı giriş bilgileri");

            // Check 2FA
            if (user.TwoFactorEnabled)
            {
                var code = new Random().Next(100000, 999999).ToString();
                user.TwoFactorCode = code;
                user.TwoFactorCodeExpiry = DateTime.Now.AddMinutes(3); // 3 min expiry
                await _unitOfWork.Users.UpdateAsync(user);
                try
                {
                    await _emailService.SendAsync(user.Email, "Giriş Doğrulama Kodu", $"Kodunuz: {code}");
                }
                catch (Exception ex)
                {
                    return new ErrorDataResult<User>("2FA kod gönderilirken hata oluştu: " + ex.Message);
                }

                // Return success but with a specific message indicating 2FA is needed. 
                // The Controller will handle not issuing the token yet.
                return new SuccessDataResult<User>(user, "2FA_REQUIRED");
            }

            return new SuccessDataResult<User>(user, "Giriş başarılı");
        }

        public async Task<IResult> RegisterAsync(UserRegisterDto userRegisterDto)
        {
            var exists = await _unitOfWork.Users.GetAsync(u => u.Username == userRegisterDto.Username);
            if (exists != null) return new ErrorResult("Username already exists");

            var res = _userValidator.ValidateAsync(userRegisterDto).GetAwaiter().GetResult();
            if (!res.IsValid)
            {
                return new ErrorResult(res.Errors.FirstOrDefault()!.ToString());
            }

            var user = new User
            {
                Username = userRegisterDto.Username,
                FullName = userRegisterDto.FullName,
                Email = userRegisterDto.Email,
                PasswordHash = PasswordHasher.Hash(userRegisterDto.Password),
                Role = "User"
            };

            await _unitOfWork.Users.AddAsync(user);
            return new SuccessResult("Kullanıcı eklendi");
        }

        public async Task<IDataResult<User>> VerifyTwoFactorAsync(VerifyTwoFactorDto verifyTwoFactorDto)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Username == verifyTwoFactorDto.Username || u.Email == verifyTwoFactorDto.Username);
            if (user == null) return new ErrorDataResult<User>("Kullanıcı bulunamadı");

            if (user.TwoFactorCode != verifyTwoFactorDto.Code || user.TwoFactorCodeExpiry < DateTime.Now)
            {
                return new ErrorDataResult<User>("Hatalı veya süresi dolmuş kod");
            }

            // Code valid, clear it
            user.TwoFactorCode = null;
            user.TwoFactorCodeExpiry = null;
            await _unitOfWork.Users.UpdateAsync(user);

            return new SuccessDataResult<User>(user, "Giriş başarılı");
        }

        public async Task<IResult> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Email == forgotPasswordDto.Email);
            if (user == null) return new ErrorResult("Kullanıcı bulunamadı");

            var token = new Random().Next(100000, 999999).ToString();
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = DateTime.Now.AddMinutes(15);
            await _unitOfWork.Users.UpdateAsync(user);

            await _emailService.SendAsync(user.Email, "Şifre Sıfırlama Kodu", $"Kodunuz: {token}");
            return new SuccessResult("Şifre sıfırlama kodu gönderildi");
        }

        public async Task<IResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Email == resetPasswordDto.Email);
            if (user == null) return new ErrorResult("Kullanıcı bulunamadı");

            if (user.PasswordResetToken != resetPasswordDto.Token || user.PasswordResetTokenExpiry < DateTime.Now)
            {
                return new ErrorResult("Geçersiz veya süresi dolmuş kod");
            }

            user.PasswordHash = PasswordHasher.Hash(resetPasswordDto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            await _unitOfWork.Users.UpdateAsync(user);

            return new SuccessResult("Şifre başarıyla güncellendi");
        }

        public async Task<IResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Id == changePasswordDto.UserId);
            if (user == null) return new ErrorResult("Kullanıcı bulunamadı");

            if (!PasswordHasher.Verify(changePasswordDto.OldPassword, user.PasswordHash))
            {
                return new ErrorResult("Eski şifre hatalı");
            }

            user.PasswordHash = PasswordHasher.Hash(changePasswordDto.NewPassword);
            await _unitOfWork.Users.UpdateAsync(user);
            return new SuccessResult("Şifre değiştirildi");
        }

        public async Task<IResult> UpdateProfileAsync(UserProfileUpdateDto userProfileUpdateDto)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Id == userProfileUpdateDto.Id);
            if (user == null) return new ErrorResult("Kullanıcı bulunamadı");

            user.FullName = userProfileUpdateDto.FullName;
            user.Email = userProfileUpdateDto.Email;
            user.TwoFactorEnabled = userProfileUpdateDto.TwoFactorEnabled;

            await _unitOfWork.Users.UpdateAsync(user);
            return new SuccessResult("Profil güncellendi");
        }

        public async Task<IDataResult<User>> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Id == id);
            if (user == null) return new ErrorDataResult<User>("Kullanıcı bulunamadı");
            return new SuccessDataResult<User>(user);
        }

        public async Task<IResult> SetRefreshTokenAsync(string refreshToken, DateTime refreshTokenExpiryTime, int id)
        {
            return await HandleWithTransactionHelper.Handling(async () =>
            {
                var user = await _unitOfWork.Users.GetAsync(p => p.Id == id);
                if (user == null)
                    return new ErrorResult("Refresh token verilecek kullanıcı bulunamadı");
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
                await _unitOfWork.Users.UpdateAsync(user);
                return new SuccessResult("Refresh işlemi başarılı");
            }, _unitOfWork);
        }
    }
}
