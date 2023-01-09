namespace Core.DTO_Models;

public class UpdateCustomer
{
    public Guid CustomerId { get; set; }
    public string Name { get; set; } 
    public string Surname { get; set; } 
    public string? MiddleName { get; set; }
    public string Sex { get; set; } 
    public DateTime Dob { get; set; }
    public DateTime Dor { get; set; }
    public string City { get; set; } 
    public string Address { get; set; } 
    public int Zip { get; set; }
    public string Phone { get; set; } 
}