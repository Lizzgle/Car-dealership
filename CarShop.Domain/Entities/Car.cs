using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Domain.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; } = null;
        //public string Description { get; set; } = null;

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public CarCategory? Category { get; set; }

        public string? Image {  get; set; }

    }
}
