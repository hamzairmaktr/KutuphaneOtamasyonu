using Core.Entities;

namespace IKitaplik.Entities.DTOs.UserDTOs
{
    public class ResetPasswordDto : IDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
