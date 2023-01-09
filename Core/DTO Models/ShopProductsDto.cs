namespace Core.DTO_Models;

public class ShopProductsDto
{
    public Guid? ProductId { get; set; }
    public Guid ShopId { get; set; }
    public int Count { get; set; }
}