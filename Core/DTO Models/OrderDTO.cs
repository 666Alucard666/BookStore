
namespace Core.DTO_Models;

public class OrderDTO
{
    public Guid CustomerId { get; set; }
    public decimal Sum { get; set; }
    public string RecipientName { get; set; }
    public string RecipientSurname { get; set; }
    public string RecipientCity { get; set; }
    public string RecipientAddress { get; set; }
    public string RecipientPhone { get; set; }
    public string PaymentType { get; set; }
    public string PaymentStatus { get; set; }
    public Guid? ShopId { get; set; }
    public List<ProductBasket> ProductsList { get; set; }
}