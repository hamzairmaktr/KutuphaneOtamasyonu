using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.DTOs.BookDTOs
{
    public class BookAddPieceDto:IDto
    {
        public int Id { get; set; }
        public int BeAdded { get; set; }
    }
}
