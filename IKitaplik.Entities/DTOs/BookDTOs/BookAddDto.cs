using Core.Entities;
using IKitaplik.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.DTOs.BookDTOs
{
    public class BookAddDto:IDto
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        public int WriterId { get; set; }
        public int CategoryId { get; set; }
        public string ShelfNo { get; set; }
        public int Piece { get; set; }
        public bool Situation { get; set; }
        public int PageSize { get; set; }
    }
}
