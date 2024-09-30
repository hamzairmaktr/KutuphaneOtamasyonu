using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplık.Entities.Concrete
{
    public class Movement:BaseEntities
    {
        public DateTime MovementDate { get; set; } = DateTime.Now;
        public string Title { get; set; }
        public string Note { get; set; }
        public int Type { get; set; } // 1-Deposit 2-Book 3-Student 5-Users
        public int? StudentId { get; set; }
        public int? BookId { get; set; }
        public int? DonationId { get; set; }
        public int? DepositId { get; set; }

        public Deposit? Deposit { get; set; } = null;
        public Donation? Donation { get; set; } = null;
        public Book? Book { get; set; } = null;
        public Student? Student { get; set; } = null;
    }
}
