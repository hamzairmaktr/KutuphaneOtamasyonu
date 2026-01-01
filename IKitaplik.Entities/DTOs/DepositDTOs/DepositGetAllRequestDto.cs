using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.DTOs.DepositDTOs
{
    public class DepositGetAllRequestDto : IDto
    {
        public bool IncludeDelivered { get; set; }
    }
}
