using Core.Models;

namespace Core.DTO_Models;

public class OrderDTO
{
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public DateTime Date { get; set; }
    public User User { get; set; }
    public ICollection<Book> Books {get; set; }
}