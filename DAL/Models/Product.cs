using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
            ShopProducts = new HashSet<ShopProduct>();
        }

        public Guid ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string ProducingCountry { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime ProducingDate { get; set; }
        public string ProducingCompany { get; set; } = null!;
        public string? Image { get; set; }

        public virtual ProductInfo ProductInfo { get; set; } = null!;
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual ICollection<ShopProduct> ShopProducts { get; set; }
    }
}
