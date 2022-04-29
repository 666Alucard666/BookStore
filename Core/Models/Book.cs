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
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [MaxLength(30)]
        public string Genre { get; set; }

        [Required]
        [MaxLength(60)]
        public string Author { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(30)]
        public string Publishing { get; set; }
        [Required]
        public int AmountOnStore { get; set; }
        public ICollection<OrdersBooks> OrdersBook { get; set; }
    }
}
