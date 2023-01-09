using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class NailsCareProduct
    {
        public Guid NailsCareProductId { get; set; }
        public string? NailsType { get; set; }
        public string? NailsDisease { get; set; }
        public string? Fragrance { get; set; }

        public virtual ProductInfo NailsCareProductNavigation { get; set; } = null!;
    }
}
