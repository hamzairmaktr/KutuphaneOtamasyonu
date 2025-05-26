using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.DTOs.DepositDTOs
{
    public class DepositAddDto:IDto
    {
        public int BookId { get; set; }
        public int StudentId { get; set; }
        public DateTime IssueDate { get; set; }
        public string Note { get; set; }
    }
}
