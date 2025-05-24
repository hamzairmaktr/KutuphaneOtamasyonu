using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.DTOs.BookDTOs
{
    public class BookGetDTO : IDto
    {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string WriterName { get; set; }
        public string CategoryName { get; set; }
        public string ShelfNo { get; set; }
        public int Piece { get; set; }
        public bool Situation { get; set; }
        public int PageSize { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
