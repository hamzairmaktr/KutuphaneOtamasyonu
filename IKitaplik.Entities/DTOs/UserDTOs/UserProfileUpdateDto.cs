using Core.Entities;

namespace IKitaplik.Entities.DTOs.UserDTOs
{
    public class UserProfileUpdateDto : IDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}
