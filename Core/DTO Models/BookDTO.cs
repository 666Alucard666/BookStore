using System.ComponentModel.DataAnnotations;

namespace Core.DTO_Models;

public class BookDTO
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    public string Genre { get; set; }

    public string Author { get; set; }

    public double Price { get; set; }

    public string Publishing { get; set; }
    public int AmountOnStore { get; set; }
}