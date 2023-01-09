using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class OrdersBooks
{
    [Key, Column(Order = 0)]
    public int BookId { get; set; }
    [Key, Column(Order = 1)]
    public int OrderId { get; set; }
    public virtual Book Book { get; set; }
    public virtual Order Order { get; set; }

    public int Count { get; set; }
}