using Core.Entities;

namespace IKitaplik.Entities.DTOs.UserDTOs
{
    public class VerifyTwoFactorDto : IDto
    {
        public string Username { get; set; }
        public string Code { get; set; }
    }
}
