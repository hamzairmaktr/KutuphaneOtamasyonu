using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplık.Entities.DTOs
{
    public class MovementGetDTO:IDto
    {
        public int Id { get; set; }
        public DateTime MovementDate { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public int Type { get; set; }
        public string StudentName { get; set; }
        public string BookName { get; set; }

        public int? StudentId { get; set; }
        public int? BookId { get; set; }
        public int? DepositId { get; set; }
        public int? DonationId { get; set; }
    }
}
