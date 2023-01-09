namespace Core.DTO_Models;

public class UserDTO
{
    public string Password { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? MiddleName { get; set; }
    public string Sex { get; set; }
    public DateTime Dob { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public int? Zip { get; set; }
    public string? Phone { get; set; }
    public int? Salary { get; set; }
    public string Specialty { get; set; }
    public Guid? ShopId { get; set; }
}