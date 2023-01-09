using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Shop
    {
        public Shop()
        {
            Orders = new HashSet<Order>();
            ShopProducts = new HashSet<ShopProduct>();
            Workers = new HashSet<Worker>();
        }

        public Guid ShopId { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Region { get; set; } = null!;
        public TimeSpan StartWorkingHours { get; set; }
        public TimeSpan EndWorkingHours { get; set; }
        public string Size { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ShopProduct> ShopProducts { get; set; }
        public virtual ICollection<Worker> Workers { get; set; }
    }
}
