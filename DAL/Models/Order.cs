using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public Guid OrderId { get; set; }
        public decimal Sum { get; set; }
        public string RecipientName { get; set; } = null!;
        public string RecipientSurname { get; set; } = null!;
        public string RecipientCity { get; set; } = null!;
        public string RecipientAddress { get; set; } = null!;
        public string RecipientPhone { get; set; } = null!;
        public string PaymentType { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public Guid CustomerId { get; set; }
        public Guid? ShopId { get; set; }
        public DateTime ProcessedDate { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Shop? Shop { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
