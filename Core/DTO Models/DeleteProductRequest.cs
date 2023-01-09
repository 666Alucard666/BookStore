namespace Core.DTO_Models;

public class DeleteProductRequest
{
    public Guid ProductId { get; set; }
    public Guid ShopId { get; set; }
}