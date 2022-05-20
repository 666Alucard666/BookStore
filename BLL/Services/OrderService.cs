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
            numb += book.Book.BookId;
            sum += book.Book.Price;
        }
        var newOrder = new Order()
        {
            Address = order.Address,
            Date = order.Date,
            OrderNumber = Convert.ToInt32(new string(numb.Reverse().ToArray())),
            PhoneNumber = order.PhoneNumber,
            User = order.User,
            UserId = order.User.UserId,
            Sum = sum,
            OrdersBook = new List<OrdersBooks>(),
        };


        using ( _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                //foreach (var book in order.Books)
                //{
                //    newOrder.OrdersBook.Add(new OrdersBooks
                //    {
                //        BookId = book.Book.BookId,
                //        Count = book.Count
                //    });
                //}
                
                _unitOfWork.OrderRepository.Create(newOrder);

                // await _unitOfWork.OrderRepository.Update(newOrder);
                //_unitOfWork.UserRepository.FirstOrDefault(u => u.UserId == newOrder.UserId).Orders.Add(newOrder);
                //await _unitOfWork.UserRepository.Update(_unitOfWork.UserRepository.FirstOrDefault(u => u.UserId == newOrder.UserId));
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
                   await _unitOfWork.BookRepository.Update(book);
                }
                _unitOfWork.OrderRepository.Remove(delorder);
                _unitOfWork.UserRepository.FirstOrDefault(u => u.UserId == delorder.UserId).Orders.Remove(delorder);
                await _unitOfWork.UserRepository.Update(_unitOfWork.UserRepository.FirstOrDefault(u => u.UserId == delorder.UserId));
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
    public IEnumerable<Order> GetAllOrdersByUser(UserDTO user)
    {
        return _unitOfWork.OrderRepository.Get(o => o.User.Email == user.Email || o.User.PhoneNumber == user.PhoneNumber);
    }
}