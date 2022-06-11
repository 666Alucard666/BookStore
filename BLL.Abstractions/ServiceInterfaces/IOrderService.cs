using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ceTe.DynamicPDF;
using Core.DTO_Models;
using Core.Models;

namespace BLL.Abstractions.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<bool> MakeOrder(OrderDTO order);
        Task<bool> DeleteOrder(DeleteOrderRequest order);
        Task<Order> GetOrderByNumber(int number);
        IEnumerable<Order> GetAllOrdersByUser(int userId);
        Task<Document> CreateReceipt(int id);
    }
}
