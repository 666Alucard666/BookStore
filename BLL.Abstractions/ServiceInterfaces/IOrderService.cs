using ceTe.DynamicPDF;
using Core.DTO_Models;
using DAL.Models;

namespace BLL.Abstractions.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<bool> MakeOrder(OrderDTO order);
        Task<bool> DeleteOrder(DeleteOrderRequest order);
        Task<Order> GetOrderByNumber(Guid number);
        Task<IEnumerable<Order>> GetAllOrdersByCustomer(Guid id);
        Task<IEnumerable<Order>> GetAllOrdersByShop(Guid shopId);
        Task<Document> CreateAndSendReceipt(Guid id);
        Task<IEnumerable<OrderCities>> GetPopularRecipientCities();
    }
}
