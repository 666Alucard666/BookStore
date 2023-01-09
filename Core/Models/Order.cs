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
    public string PhoneNumber { get; set; }
    [Required]
    public string Adress { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public string Recipient { get; set; }
    public virtual User User { get; set; }
    
    public virtual ICollection<OrdersBooks> OrdersBook { get; set; }
}