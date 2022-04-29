using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class User : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(150)]
        public string Email { get; set; }
        
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(30)]
        public string Surname { get; set; }
        
        [Required]
        [MaxLength(30)]
        public string Login { get; set; }

        [Required]
        [MaxLength(30)]
        public string Password { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
