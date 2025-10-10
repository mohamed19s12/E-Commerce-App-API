﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entities.Product
{
    public class Category : EntityBase<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
