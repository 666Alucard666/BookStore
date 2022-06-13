using BLL.Abstractions.ServiceInterfaces;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using Core.DTO_Models;
using Core.Models;
using DAL.Abstractions.Interfaces;
using iTextSharp.text.pdf;

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
                    var updbook = _unitOfWork.BookRepository.FindById(book.Book.Id);
                    if (updbook.AmountOnStore-book.Count < 0)
                    {
                        return false;
                    }

                    updbook.AmountOnStore = updbook.AmountOnStore - book.Count;
                    await _unitOfWork.BookRepository.Update(updbook);
                }
                
                _unitOfWork.OrderRepository.Create(newOrder);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch 
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
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
                foreach (var book in delorder.OrdersBook)
                {
                    var updbook = _unitOfWork.BookRepository.FindById(book.BookId);

                    updbook.AmountOnStore = updbook.AmountOnStore + book.Count;
                    await _unitOfWork.BookRepository.Update(updbook);
                }
                _unitOfWork.OrderRepository.Remove(delorder);
                await _unitOfWork.SaveAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
        }
        return true;
    }

    public async Task<Order> GetOrderByNumber(int number)
    {
        return await _unitOfWork.OrderRepository.FirstOrDefaultAsync(o=>o.OrderNumber == number);
    }

    public async Task<Document> CreateReceipt(int id)
    {
        var order = await _unitOfWork.OrderRepository.FirstOrDefaultAsync(o => o.OrderId == id);
        var books = new List<Book>();
        var sum = 0d;
        foreach (var ob in order.OrdersBook)
        {
            var b = _unitOfWork.BookRepository.FindById(ob.BookId);
            sum += b.Price*ob.Count;
            books.Add(b);
        }
        Document document = new Document();
            
        Page page = new Page();
        document.Pages.Add(page);
            
        Table2 table = new Table2(0, 100, 540, 200);
        
        table.Columns.Add(70);
        table.Columns.Add(100);
        table.Columns.Add(150);
        table.Columns.Add(100);
        table.Columns.Add(70);
        table.Columns.Add(50);
            
        Row2 row1 = table.Rows.Add(100, Font.HelveticaBold, 16, Grayscale.Black, Grayscale.Gray);
        row1.CellDefault.Align = TextAlign.Center;
        row1.CellDefault.VAlign = VAlign.Center;
        row1.Cells.Add("Number");
        row1.Cells.Add("Recipient");
        row1.Cells.Add("Phone number");
        row1.Cells.Add("Address");
        row1.Cells.Add("Creating date");
        row1.Cells.Add("Total sum");

            
        Row2 row2 = table.Rows.Add(100);
        Cell2 cell1 = row2.Cells.Add($"{order.OrderNumber}", Font.HelveticaBold, 16, Grayscale.Black, Grayscale.Gray, 1);
        cell1.Align = TextAlign.Center;
        cell1.VAlign = VAlign.Center;
        row2.Cells.Add($"{order.Recipient}");
        row2.Cells.Add($"{order.PhoneNumber}");
        row2.Cells.Add($"{order.Adress}");
        row2.Cells.Add($"{order.Date}");
        row2.Cells.Add($"{Math.Round(sum,2)}$");
        

        Table2 table2 = new Table2(0, 400, 540, 50*books.Count);
        
        table2.Columns.Add(70);
        table2.Columns.Add(100);
        table2.Columns.Add(150);
        table2.Columns.Add(100);
        table2.Columns.Add(70);
        table2.Columns.Add(50);
        
        Row2 row12 = table.Rows.Add(100, Font.HelveticaBold, 16, Grayscale.Black, Grayscale.Gray);
        row12.CellDefault.Align = TextAlign.Center;
        row12.CellDefault.VAlign = VAlign.Center;
        row12.Cells.Add("Title");
        row12.Cells.Add("Genre");
        row12.Cells.Add("Author");
        row12.Cells.Add("Publishing");
        row12.Cells.Add("Count");
        row12.Cells.Add("Price");
        var i = 0;
        foreach (var book in books)
        {
            table2.Rows.Add(50);
            table2.Rows[i].Cells.Add($"{book.Name}");
            table2.Rows[i].Cells.Add($"{book.Genre}");
            table2.Rows[i].Cells.Add($"{book.Author}");
            table2.Rows[i].Cells.Add($"{book.Publishing}");
            table2.Rows[i].Cells.Add($"{order.OrdersBook.FirstOrDefault(ob=>ob.BookId==book.BookId).Count}");
            table2.Rows[i].Cells.Add($"{order.OrdersBook.FirstOrDefault(ob=>ob.BookId==book.BookId).Count * book.Price}$");
            i++;
        }
        
        page.Elements.Add(table);
        page.Elements.Add(table2);
            
        document.Draw($@"{Environment.CurrentDirectory}\Receipt.pdf");
        return document;
    }
    
    public IEnumerable<Order> GetAllOrdersByUser(int userId)
    {
        return _unitOfWork.OrderRepository.Get(o => o.UserId == userId);
    }
}