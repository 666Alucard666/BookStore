namespace Core.DTO_Models;

public class ShopDto
{
    public Guid? ShopId { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string Region { get; set; }
    public float StartWorkingHours { get; set; }
    public float EndWorkingHours { get; set; }
    public string Size { get; set; }
}