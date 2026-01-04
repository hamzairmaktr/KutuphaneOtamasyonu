using Core.Entities;

namespace IKitaplik.Entities.DTOs.UserDTOs
{
    public class ForgotPasswordDto : IDto
    {
        public string Email { get; set; }
    }
}
