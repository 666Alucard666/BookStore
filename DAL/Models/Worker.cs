using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Worker
    {
        public Guid WorkerId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string Sex { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int Salary { get; set; }
        public string Specialty { get; set; } = null!;
        public Guid? ShopId { get; set; }

        public virtual Shop? Shop { get; set; }
        public virtual User WorkerNavigation { get; set; } = null!;
    }
}
