
namespace Core.DTO_Models;

public class OrderDTO
{
    public string PhoneNumber { get; set; }
    public string Adress { get; set; }
    public DateTime Date { get; set; }
    public int UserId { get; set; }
    public int OrderNumber { get; set; }
    public double Sum { get; set; }
    public string Recipient { get; set; }
    public ICollection<BookBasket> Books {get; set; }
}