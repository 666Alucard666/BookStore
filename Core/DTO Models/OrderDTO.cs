using Core.Models;

namespace Core.DTO_Models;

public class OrderDTO
{
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public DateTime Date { get; set; }
    public User User { get; set; }
    public int OrderNumber { get; set; }
    public ICollection<BookBasket> Books {get; set; }
}