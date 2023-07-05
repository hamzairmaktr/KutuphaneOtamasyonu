﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplık.Entities.Concrete
{
    public class Category:BaseEntities
    {
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
