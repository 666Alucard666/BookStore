using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class ProductInfo
    {
        public Guid ProductInfoId { get; set; }
        public string Instruction { get; set; } = null!;
        public int Capacity { get; set; }
        public string? Contraindication { get; set; }
        public string Category { get; set; } = null!;
        public string Gender { get; set; } = null!;

        public virtual Product ProductInfoNavigation { get; set; } = null!;
        public virtual HairCareProduct HairCareProduct { get; set; } = null!;
        public virtual NailsCareProduct NailsCareProduct { get; set; } = null!;
        public virtual OralCavityProduct OralCavityProduct { get; set; } = null!;
        public virtual SkinCareProduct SkinCareProduct { get; set; } = null!;
    }
}
