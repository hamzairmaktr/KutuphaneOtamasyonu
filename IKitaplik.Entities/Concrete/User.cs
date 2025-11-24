using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.Concrete
{
    public class User : IEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "User";
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public ICollection<Book> Books { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Writer> Writers { get; set; }
        public ICollection<Deposit> Deposits { get; set; }
        public ICollection<Donation> Donations { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Movement> Movements { get; set; }
    }
}
