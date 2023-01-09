using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class User
    {
        public Guid UserId { get; set; }
        public string? HashPassword { get; set; }
        public string Email { get; set; } = null!;

        public virtual Customer Customer { get; set; } = null!;
        public virtual Worker Worker { get; set; } = null!;
    }
}
