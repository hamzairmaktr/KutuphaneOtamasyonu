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

        public IDataResult<User> Login(UserLoginDto userLoginDto)
        {
            var user = _unitOfWork.Users.Get(u => u.Username == userLoginDto.Username);

            if (user == null || !PasswordHasher.Verify(userLoginDto.Password, user.PasswordHash))
                return new ErrorDataResult<User>("Hatalı giriş bilgileri");
            return new SuccessDataResult<User>(user,"Giriş başarılı");
        }

        public IResult Register(UserRegisterDto userRegisterDto)
        {
            var exists = _unitOfWork.Users.Get(u => u.Username == userRegisterDto.Username);
            if (exists != null) return new ErrorResult("Username already exists");

            var user = new User
            {
                Username = userRegisterDto.Username,
                FullName = userRegisterDto.FullName,
                Email = userRegisterDto.Email,
                PasswordHash = PasswordHasher.Hash(userRegisterDto.Password),
                Role = "User"
            };

            _unitOfWork.Users.Add(user);
            return new SuccessResult("Kullanıcı eklendi");
        }
    }
}
