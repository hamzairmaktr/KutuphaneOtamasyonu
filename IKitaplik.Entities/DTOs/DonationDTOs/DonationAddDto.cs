using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.DTOs.DonationDTOs
{
    public class DonationAddDto:IDto
    {
        public int StudentId { get; set; }
        public DateTime Date { get; set; }
        public bool? IsItDamaged { get; set; }
    }
}
