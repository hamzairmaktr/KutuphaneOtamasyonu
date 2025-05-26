using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.DTOs.DonationDTOs
{
    public class DonationGetDTO:IDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string StudentName { get; set; }
        public string BookName { get; set; }
        public string BookBarcode { get; set; }
        public bool? IsItDamaged { get; set; }
    }
}
