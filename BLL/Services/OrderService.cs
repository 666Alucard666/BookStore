using BLL.Abstractions.ServiceInterfaces;
using Core.DTO_Models;
using Core.Models;
using DAL.Abstractions.Interfaces;

namespace BLL.Services;

public class OrderService : IOrderService
{
    private IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<bool> MakeOrder(OrderDTO order)
    {
        if (order==null || 
            order.Books.Count==0)
        {
            return false;
        }

        var numb = string.Empty;
        decimal sum = 0;
        foreach (var book in order.Books)
        {
            numb += book.BookId;
            sum += book.Price;
        }
        var newOrder = new Order()
        {
            Address = order.Address,
            Date = order.Date,
            OrderNumber = Convert.ToInt32(numb.Reverse().ToString()),
            PhoneNumber = order.PhoneNumber,
            User = order.User,
            UserId = order.User.UserId,
            Sum = sum,
            //OrdersBook = order.Books.Select(b=>new OrdersBooks { BookId=b.BookId}).ToList(),
        };
       
        
        using (_unitOfWork.BeginTransactionAsync())
        {
            try
            { 
                _unitOfWork.OrderRepository.Create(newOrder);
                await _unitOfWork.SaveAsync();
                    
                await _unitOfWork.CommitTransactionAsync();
            }
            catch 
            {
                await _unitOfWork.RollbackTransactionAsync();
            }
        }

        return true;
    }

    public async Task<bool> DeleteOrder(OrderDTO order)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderDTO> GetOrderByNumber(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderDTO> GetOrderByUser(UserDTO user)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<OrderDTO>> GetAllOrdersByUser(UserDTO user)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> EditOrder(Order order, OrderDTO orderDto)
    {
        throw new NotImplementedException();
    }
}