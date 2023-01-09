using System.Net.Mail;
using BLL.Abstractions.ServiceInterfaces;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using Core.DTO_Models;
using Core.Models;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Order = DAL.Models.Order;

namespace BLL.Services;

public class OrderService : IOrderService
{
    private readonly GigienaStoreDbContext _context;

    public OrderService(GigienaStoreDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> MakeOrder(OrderDTO order)
    {
        if (!order.ProductsList.Any())
        {
            return false;
        }

        var orderModel = new Order
        {
            OrderId = Guid.NewGuid(),
            CustomerId = order.CustomerId,
            Sum = order.Sum,
            RecipientName = order.RecipientName,
            RecipientSurname = order.RecipientSurname,
            RecipientCity = order.RecipientCity,
            RecipientAddress = order.RecipientAddress,
            RecipientPhone = order.RecipientPhone,
            PaymentType = order.PaymentType,
            PaymentStatus = "Pending",
            ShopId = order.ShopId,
            ProcessedDate = DateTime.Now
        };

        var orderProds = order.ProductsList.Select(x => new OrderProduct
        {
            OrderId = orderModel.OrderId,
            ProductId = x.ProductId,
            Count = x.Count
        });
        foreach (var orderProduct in orderProds)
        {
            var prod = _context.Products.Include(x => x.ShopProducts).First(x => x.ProductId == orderProduct.ProductId);
            foreach (var shopProduct in prod.ShopProducts)
            {
                if (orderProduct.Count/prod.ShopProducts.Count > 0)
                {
                    shopProduct.Count -= orderProduct.Count/prod.ShopProducts.Count;
                }
                else
                {
                    shopProduct.Count -= orderProduct.Count;
                    break;
                }
            }

            _context.Update(prod);
        }
        try
        {
            await _context.Orders.AddAsync(orderModel);
            await _context.OrderProducts.AddRangeAsync(orderProds);
            await _context.SaveChangesAsync();
            await CreateAndSendReceipt(orderModel.OrderId);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> DeleteOrder(DeleteOrderRequest order)
    {
        var orderModel = await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == order.OrderId);
        if (orderModel is not null)
        {
            _context.Orders.Remove(orderModel);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<Order> GetOrderByNumber(Guid number)
    {
        return await _context.Orders.AsNoTracking().Include(x => x.Customer).ThenInclude(x => x.CustomerNavigation).Include(x => x.Shop).Include(x => x.OrderProducts).ThenInclude(x => x.Product).ThenInclude(x => x.ProductInfo).FirstOrDefaultAsync(x => x.OrderId == number);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersByCustomer(Guid id)
    {
        return _context.Orders.AsNoTracking().Include(x => x.Shop).Include(x => x.OrderProducts).ThenInclude(x => x.Product).Where(x => x.CustomerId == id);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersByShop(Guid shopId)
    {
        return _context.Orders.AsNoTracking().Include(x => x.OrderProducts).Where(x => x.ShopId == shopId);
    }

    public async Task<Document> CreateAndSendReceipt(Guid id)
    {
        var order = await GetOrderByNumber(id);
        decimal sum = 0;
        foreach (var orderProduct in order.OrderProducts)
        {
            sum += orderProduct.Count * orderProduct.Product.Price;
        }

        Document document = new Document();

        Page page = new Page();
        document.Pages.Add(page);
        var page2 = new Page();
        document.Pages.Add(page2);


        Table2 table = new Table2(-50, 100, 700, 400);
        
        table.Columns.Add(70);
        table.Columns.Add(100);
        table.Columns.Add(70);
        table.Columns.Add(100);
        table.Columns.Add(100);
        table.Columns.Add(70);
        table.Columns.Add(50);
        if (order.Shop is not null)
        {
            table.Columns.Add(150);
        }
        
        Row2 row1 = table.Rows.Add(100, Font.HelveticaBold, 16, Grayscale.Black, Grayscale.Gray);
        row1.CellDefault.Align = TextAlign.Center;
        row1.CellDefault.VAlign = VAlign.Center;
        row1.Cells.Add("Number");
        row1.Cells.Add("Recipient");
        row1.Cells.Add("Phone number");
        row1.Cells.Add("City");
        row1.Cells.Add("Address");
        row1.Cells.Add("Creating date");
        row1.Cells.Add("Total sum");
        if (order.Shop is not null)
        {
            row1.Cells.Add("Shop");
        }

            
        Row2 row2 = table.Rows.Add(100);
        Cell2 cell1 = row2.Cells.Add($"{order.OrderId}", Font.HelveticaBold, 16, Grayscale.Black, Grayscale.Gray, 1);
        cell1.Align = TextAlign.Center;
        cell1.VAlign = VAlign.Center;
        row2.Cells.Add($"{order.RecipientName } {order.RecipientSurname}");
        row2.Cells.Add($"{order.RecipientPhone}");
        row2.Cells.Add($"{order.RecipientCity}");
        row2.Cells.Add($"{order.RecipientAddress}");
        row2.Cells.Add($"{order.ProcessedDate:MM/dd/yyyy}");
        row2.Cells.Add($"{Math.Round(sum,2)}$");
        if (order.Shop is not null)
        {
            row2.Cells.Add($"{order.Shop.Name}");
        }

        Table2 table2 = new Table2(0, 100, 700, 150*order.OrderProducts.Count);
        
        table2.Columns.Add(70);
        table2.Columns.Add(100);
        table2.Columns.Add(70);
        table2.Columns.Add(100);
        table2.Columns.Add(100);
        table2.Columns.Add(50);
        
        Row2 row12 = table.Rows.Add(100, Font.HelveticaBold, 16, Grayscale.Black, Grayscale.Gray);
        row12.CellDefault.Align = TextAlign.Center;
        row12.CellDefault.VAlign = VAlign.Center;
        row12.Cells.Add("Product name");
        row12.Cells.Add("Category");
        row12.Cells.Add("Producing company");
        row12.Cells.Add("Producing country");
        row12.Cells.Add("Count");
        row12.Cells.Add("Price");
        var i = 0;
        foreach (var book in order.OrderProducts)
        {
            table2.Rows.Add(150);
            table2.Rows[i].Cells.Add($"{book.Product.Name}");
            table2.Rows[i].Cells.Add($"{book.Product.ProductInfo.Category}");
            table2.Rows[i].Cells.Add($"{book.Product.ProducingCompany}");
            table2.Rows[i].Cells.Add($"{book.Product.ProducingCountry}");
            table2.Rows[i].Cells.Add($"{book.Count}");
            table2.Rows[i].Cells.Add($"{book.Count * book.Product.Price}$");
            i++;
        }
        
        page.Elements.Add(table);
        page2.Elements.Add(table2);
            
        document.Draw($@"{Environment.CurrentDirectory}\Receipt.pdf");
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
        mail.From = new MailAddress("dmytro.vakhnenko@nure.ua");
        mail.To.Add(order.Customer.CustomerNavigation.Email);
        mail.Subject = $"Your receipt {order.OrderId}";
        mail.Body = "Thanks for being with us";

        Attachment attachment;
        attachment = new Attachment($@"{Environment.CurrentDirectory}\Receipt.pdf");
        mail.Attachments.Add(attachment);

        SmtpServer.Port = 587;
        SmtpServer.Credentials = new System.Net.NetworkCredential("dmytro.vakhnenko@nure.ua", "zjXoS9jz");
        SmtpServer.EnableSsl = true;

        SmtpServer.Send(mail);
        return document;
    }

    public async Task<IEnumerable<OrderCities>> GetPopularRecipientCities()
    {
        return await _context.Orders.Where(x => x.ProcessedDate.Month == DateTime.Now.Month).GroupBy(x => x.RecipientCity).Select(x => new OrderCities
        {
            City = x.Key, Count = x.Count()
        }).OrderByDescending(x => x.Count).Take(5).ToListAsync();
    }
}