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
        double sum = 0;
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
            OrdersBook = new List<OrdersBooks>(),
        };
       
        
        using (_unitOfWork.BeginTransactionAsync())
        {
            try
            { 
                _unitOfWork.OrderRepository.Create(newOrder);
                foreach (var book in order.Books)
                {
                    newOrder.OrdersBook.Add(new OrdersBooks
                    {
                        BookId = book.BookId,
                        OrderId = newOrder.OrderId,
                        Order = newOrder,
                        Book = book
                    });
                    _unitOfWork.BookRepository.FirstOrDefault(b=>b.BookId == book.BookId).OrdersBook = newOrder.OrdersBook;
                    _unitOfWork.BookRepository.Update(_unitOfWork.BookRepository.FirstOrDefault(b => b.BookId == book.BookId));
                }
                _unitOfWork.OrderRepository.Update(newOrder);
                _unitOfWork.UserRepository.FirstOrDefault(u => u.UserId == newOrder.UserId).Orders.Add(newOrder);
                _unitOfWork.UserRepository.Update(_unitOfWork.UserRepository.FirstOrDefault(u => u.UserId == newOrder.UserId));
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
        if (order==null || !await _unitOfWork.OrderRepository.Any(o=> o.OrderNumber == order.OrderNumber))
        {
            return false;
        }
        var delorder = _unitOfWork.OrderRepository.FirstOrDefault(o => o.OrderNumber == order.OrderNumber);

        using (_unitOfWork.BeginTransactionAsync())
        {
            try
            {
               var books = _unitOfWork.BookRepository.Get(b => b.OrdersBook.FirstOrDefault(ob => ob.OrderId == delorder.OrderId).BookId
                                                              == delorder.OrdersBook.FirstOrDefault(ob => ob.OrderId == delorder.OrderId).BookId);
                foreach (var book in books)
                {
                    foreach (var ob in book.OrdersBook)
                    {
                        if (delorder.OrderId == ob.OrderId)
                        {
                            book.OrdersBook.Remove(ob);
                        }
                    }
                    _unitOfWork.BookRepository.Update(book);
                }
                _unitOfWork.OrderRepository.Remove(delorder);
                _unitOfWork.UserRepository.FirstOrDefault(u => u.UserId == delorder.UserId).Orders.Remove(delorder);
                _unitOfWork.UserRepository.Update(_unitOfWork.UserRepository.FirstOrDefault(u => u.UserId == delorder.UserId));
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
            }
        }
        return false;
    }

    public async Task<Order> GetOrderByNumber(int number)
    {
        return await _unitOfWork.OrderRepository.FirstOrDefaultAsync(o=>o.OrderNumber == number);
    }

    public async Task<Order> GetOrderByUser(UserDTO user)
    {
        return await _unitOfWork.OrderRepository.FirstOrDefaultAsync(o => o.User.Email == user.Email || o.User.PhoneNumber == user.PhoneNumber);
    }

    public IEnumerable<Order> GetAllOrdersByUser(UserDTO user)
    {
        return _unitOfWork.OrderRepository.Get(o => o.User.Email == user.Email || o.User.PhoneNumber == user.PhoneNumber);
    }
}