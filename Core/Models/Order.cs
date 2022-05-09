using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class Order : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId { get; set; }
    [Required]
    public int OrderNumber { get; set; }
    [Required]
    public double Sum { get; set; }
    [Required]
    [MaxLength(30)]
    public string PhoneNumber { get; set; }
    [Required]
    [MaxLength(60)]
    public string Address { get; set; }
    [Required]
    public DateTime Date { get; set; }
    
    public int UserId { get; set; }
    
    public User User { get; set; }
    
    public ICollection<OrdersBooks> OrdersBook { get; set; }
}