using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.DTOs.UserDTOs
{
    public class RefreshTokenDto:IDto
    {
        public string RefreshToken { get; set; }
    }
}
