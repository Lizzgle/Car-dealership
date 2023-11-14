using CarShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Domain.Models
{
    public class CartItem
    {
        public Car Item { get; set; } = null!;

        public int Count { get; set; }
    }
}
