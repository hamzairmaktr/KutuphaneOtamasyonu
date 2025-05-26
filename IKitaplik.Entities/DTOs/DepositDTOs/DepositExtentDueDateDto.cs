using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.DTOs.DepositDTOs
{
    public class DepositExtentDueDateDto:IDto
    {
        public int DepositId { get; set; }
        public int? AdditionalDays { get; set; }
        public DateTime? Date { get; set; }
        public bool AsDate { get; set; }
    }
}
