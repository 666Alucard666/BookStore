using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO_Models;
using Core.Models;

namespace BLL.Abstractions.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<bool> MakeOrder(OrderDTO order);
        Task<bool> DeleteOrder(OrderDTO order);
        Task<Order> GetOrderByNumber(int number);
        Task<Order> GetOrderByUser(UserDTO user);
        IEnumerable<Order> GetAllOrdersByUser(UserDTO user);
    }
}
