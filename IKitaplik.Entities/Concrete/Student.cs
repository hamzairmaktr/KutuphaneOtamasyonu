using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.Concrete
{
    public class Student : BaseEntities
    {
        public int StudentNumber { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string TelephoneNumber { get; set; }
        public string EMail { get; set; }
        public int NumberofBooksRead { get; set; }
        public bool Situation { get; set; }
        public int Point { get; set; }

        public ICollection<Deposit> Deposits { get; set; }
        public ICollection<Donation> Donations { get; set; }
    }
}
