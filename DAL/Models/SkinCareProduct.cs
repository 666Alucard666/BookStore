using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class SkinCareProduct
    {
        public Guid SkinCareProductId { get; set; }
        public string? SkinType { get; set; }
        public string? UsePurpose { get; set; }
        public int AgeRestrictionsStart { get; set; }
        public int AgeRestrictionsEnd { get; set; }

        public virtual ProductInfo SkinCareProductNavigation { get; set; } = null!;
    }
}
