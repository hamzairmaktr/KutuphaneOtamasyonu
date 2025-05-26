using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.DTOs.DepositDTOs
{
    public class DepositUpdateDto:IDto
    {
        public int Id { get; set; }
        public DateTime DeliveryDate { get; set; }
        public bool AmILate { get; set; }
        public bool IsItDamaged { get; set; }
        public string Note { get; set; }
    }
}
