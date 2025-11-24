using Core.Entities;
using IKitaplik.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.Concrete
{
    public class Movement:BaseEntities
    {
        public DateTime MovementDate { get; set; } = DateTime.Now;
        public string Title { get; set; }
        public string Note { get; set; }
        public MovementType Type { get; set; } // 1-Deposit 2-Book 3-Student 4-donation 5-Users
        public int? StudentId { get; set; }
        public int? BookId { get; set; }
        public int? DonationId { get; set; }
        public int? DepositId { get; set; }

        public User User { get; set; }

        public Deposit? Deposit { get; set; } = null;
        public Donation? Donation { get; set; } = null;
        public Book? Book { get; set; } = null;
        public Student? Student { get; set; } = null;
    }
}
