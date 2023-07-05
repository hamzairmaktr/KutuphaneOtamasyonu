using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplık.Entities.Concrete
{
    public class Deposit:BaseEntities
    {
        public int BookId { get; set; }
        public int StudentId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool AmILate { get; set; }
        public bool IsItDamaged { get; set; }
        public string Note { get; set; }

        public Book Book { get; set; }
        public Student Student { get; set; }
    }
}
