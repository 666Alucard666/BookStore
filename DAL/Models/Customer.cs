using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public Guid CustomerId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string Sex { get; set; } = null!;
        public DateTime Dob { get; set; }
        public DateTime Dor { get; set; }
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int Zip { get; set; }
        public string Phone { get; set; } = null!;

        public virtual User CustomerNavigation { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
