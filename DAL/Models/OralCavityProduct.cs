using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class OralCavityProduct
    {
        public Guid OralCavityProductId { get; set; }
        public string? GumDiseaseType { get; set; }
        public bool? IsWhitening { get; set; }
        public bool? IsHerbalBase { get; set; }

        public virtual ProductInfo OralCavityProductNavigation { get; set; } = null!;
    }
}
