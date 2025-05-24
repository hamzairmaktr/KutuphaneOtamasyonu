using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Entities.DTOs.CategoryDTOs
{
    public class CategoryAddDto: IDto
    {
        public string Name { get; set; }
    }
}
