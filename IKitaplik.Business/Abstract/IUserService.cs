using Core.Utilities.Results;
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
        IResult Register(UserRegisterDto userRegisterDto);
        IResult Login(UserLoginDto userLoginDto);
    }
}
