using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ShopProduct
    {
        public Guid ProductId { get; set; }
        public Guid ShopId { get; set; }
        public int Count { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Shop Shop { get; set; } = null!;
    }
}
