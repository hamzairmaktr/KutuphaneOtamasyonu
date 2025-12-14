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

        public UserManager(IUnitOfWork unitOfWork, UserValidator userValidator)
        {
            _unitOfWork = unitOfWork;
            _userValidator = userValidator;
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
