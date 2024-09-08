using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplık.Entities.Concrete
{
    public class Donation:BaseEntities
    {
        public DateTime Date { get; set; }
        public int StudentId { get; set; }
        public int BookId { get; set; }
        public bool? IsItDamaged { get; set; }

        public Student Student { get; set; }
        public Book Book { get; set; }
    }
}
