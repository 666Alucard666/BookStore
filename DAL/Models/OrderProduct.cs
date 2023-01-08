using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class OrderProduct
    {
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public int Count { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
