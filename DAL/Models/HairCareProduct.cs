using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class HairCareProduct
    {
        public Guid HairCareProductId { get; set; }
        public string? HairType { get; set; }
        public string? HairDisease { get; set; }
        public bool? IsAntiDandruff { get; set; }
        public string? NotContains { get; set; }

        public virtual ProductInfo HairCareProductNavigation { get; set; } = null!;
    }
}
