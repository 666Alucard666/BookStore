namespace Core.DTO_Models;

public class ProductDto
{
    public Guid? ProductId { get; set; }
    public string Name { get; set; }
    public string ProducingCountry { get; set; }
    public decimal Price { get; set; }
    public DateTime ProducingDate { get; set; }
    public string ProducingCompany { get; set; }
    public string? Image { get; set; }
    public int? CountOrders { get; set; }
    public string Instruction { get; set; }
    public int Capacity { get; set; }
    public string? Contraindication { get; set; }
    public string Category { get; set; }
    public string Gender { get; set; }
    public IEnumerable<ShopProductsDto> ShopProducts { get; set; }
    public int AmountOnStore { get; set; }
    public HairDto? HairCareProduct { get; set; }
    public NailsDto? NailsCareProduct { get; set; }
    public OralCavityDto? OralCavityProduct { get; set; }
    public SkinCareDto? SkinCareProduct { get; set; }
}