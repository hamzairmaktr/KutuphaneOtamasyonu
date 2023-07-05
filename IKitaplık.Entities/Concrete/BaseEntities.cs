using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplık.Entities.Concrete
{
    public class BaseEntities:IEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
