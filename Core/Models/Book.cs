using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Book : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Publishing { get; set; }
        [Required]
        public int AmountOnStore { get; set; }
        public virtual ICollection<OrdersBooks> OrdersBook { get; set; }
    }
}
