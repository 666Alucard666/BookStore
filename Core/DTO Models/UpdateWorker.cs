namespace Core.DTO_Models;

public class UpdateWorker
{
    public Guid WorkerId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; } 
    public string? MiddleName { get; set; }
    public string Sex { get; set; }
    public DateTime Dob { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public int Salary { get; set; }
    public string Specialty { get; set; }
    public Guid? ShopId { get; set; }
}

public class CreateWorker
{
    public string Name { get; set; }
    public string Surname { get; set; } 
    public string? MiddleName { get; set; }
    public string Sex { get; set; }
    public DateTime Dob { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public int Salary { get; set; }
    public string Specialty { get; set; }
    public Guid? ShopId { get; set; }
}