using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.Concrete
{
    public class Book : BaseEntities
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int WriterId { get; set; }
        public string ShelfNo { get; set; }
        public int Piece { get; set; }
        public bool Situation { get; set; }
        public int PageSize { get; set; }
       
        public User User { get; set; }
        public Writer Writer { get; set; }
        public Category Category { get; set; }
        public ICollection<Deposit> Deposits { get; set; }
        public Donation? Donation { get; set; }
        public ICollection<Movement> Movements { get; set; }
    }
}
