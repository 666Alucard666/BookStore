namespace Core.DTO_Models;

public class UpdateModel
{
    public IEnumerable<UpdateCustomer>? CustomersToUpdate { get; set; }
    public IEnumerable<UpdateWorker>? WorkersToUpdate { get; set; }
    public IEnumerable<Guid>? WorkersToDelete { get; set; }
    public UserDTO? WorkerToCreate { get; set; }
}