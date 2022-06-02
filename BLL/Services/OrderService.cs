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
        var nu = new Random();
        var newOrder = new Order()
        {
            Adress = order.Adress,
            Date = order.Date,
            OrderNumber = nu.Next(),
            PhoneNumber = order.PhoneNumber,
            UserId = order.UserId,
            Sum = order.Sum,
            Recipient = order.Recipient,
            OrdersBook = new List<OrdersBooks>(),
        };


        using ( _unitOfWork.BeginTransactionAsync())
        {
            try
            { 
                foreach (var book in order.Books)
                {
                    newOrder.OrdersBook.Add(new OrdersBooks
                    {
                        BookId = book.Book.Id,
                        OrderId = newOrder.OrderId,
                        Count = book.Count,
                    });
                }
                
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

    public async Task<bool> DeleteOrder(DeleteOrderRequest order)
    {
        if (order==null || !await _unitOfWork.OrderRepository.Any(o=> o.OrderNumber == order.OrderNumber))
        {
            return false;
        }
        var delorder = _unitOfWork.OrderRepository.FirstOrDefault(o => o.OrderNumber == order.OrderNumber && o.OrderId == order.OrderId);
        if (delorder == null)
        {
            return false;
        }
        using (_unitOfWork.BeginTransactionAsync())
        {
            try
            {
                _unitOfWork.OrderRepository.Remove(delorder);
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

    public async Task<Order> GetOrderByNumber(int number)
    {
        return await _unitOfWork.OrderRepository.FirstOrDefaultAsync(o=>o.OrderNumber == number);
    }
    public IEnumerable<Order> GetAllOrdersByUser(int userId)
    {
        return _unitOfWork.OrderRepository.Get(o => o.UserId == userId);
    }
}