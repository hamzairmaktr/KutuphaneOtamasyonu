﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.DTOs.BookDTOs
{
    public class BookUpdateDto
    {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Writer { get; set; }
        public int CategoryId { get; set; }
        public string ShelfNo { get; set; }
        public int Piece { get; set; }
        public bool Situation { get; set; }
        public int PageSize { get; set; }
    }
}
