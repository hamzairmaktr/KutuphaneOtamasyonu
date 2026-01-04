using Core.Entities;

namespace IKitaplik.Entities.DTOs.UserDTOs
{
    public class ChangePasswordDto : IDto
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
