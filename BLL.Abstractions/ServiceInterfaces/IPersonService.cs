using Core.DTO_Models;
using DAL.Models;

namespace BLL.Abstractions.ServiceInterfaces;

public interface IPersonService
{
    Task<Worker> GetWorkerById(Guid id);
    Task<Customer> GetCustomerById(Guid id);
    Task<List<Worker>> GetWorkersByShop(Guid shopId);
    Task<bool> CreateWorker(CreateWorker worker);
    Task<bool> UpdateWorkers(IEnumerable<UpdateWorker> dto);
    Task<bool> UpdateCustomer(IEnumerable<UpdateCustomer> dto);
    Task<bool> DeleteWorkerById(Guid id);
    Task<bool> GetMonthStatistic();
}