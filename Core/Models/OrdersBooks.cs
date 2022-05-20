using System.ComponentModel.DataAnnotations;

namespace Core.Models;

public class OrdersBooks
{
    [Key]
    public int BookId { get; set; }
    [Key]
    public int OrderId { get; set; }
    public virtual Book Book { get; set; }
    public virtual Order Order { get; set; }
}