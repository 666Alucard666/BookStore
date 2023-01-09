using BLL.Abstractions.ServiceInterfaces;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using Core.DTO_Models;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class PersonService : IPersonService
{
    private readonly GigienaStoreDbContext _context;
    private readonly IShopService _shopService;
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;

    public PersonService(GigienaStoreDbContext context, IShopService shopService, IProductService productService, IOrderService orderService)
    {
        _context = context;
        _shopService = shopService;
        _productService = productService;
        _orderService = orderService;
    }

    public async Task<Worker> GetWorkerById(Guid id)
    {
        return await _context.Workers.Include(x => x.Shop).FirstOrDefaultAsync(x => x.WorkerId == id);
    }

    public async Task<Customer> GetCustomerById(Guid id)
    {
        return await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == id);
    }

    public Task<List<Worker>> GetWorkersByShop(Guid shopId)
    {
        return _context.Workers.AsNoTracking().Where(x => x.ShopId == shopId).ToListAsync();
    }

    public async Task<bool> CreateWorker(CreateWorker worker)
    {
        var workerNew = new Worker
        {
            WorkerId = Guid.NewGuid(),
            Name = worker.Name,
            Surname = worker.Surname,
            MiddleName = worker.MiddleName,
            Sex = worker.Sex,
            Dob = worker.Dob,
            City = worker.City,
            Address = worker.Address,
            Salary = worker.Salary,
            Specialty = worker.Specialty,
            ShopId = worker.ShopId
        };
        await _context.AddAsync(workerNew);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateWorkers(IEnumerable<UpdateWorker> dto)
    {
        try
        {
            foreach (var update in dto)
            {
                var workToUpd = await _context.Workers.FirstOrDefaultAsync(x => x.WorkerId == update.WorkerId);
                workToUpd.Address = update.Address;
                workToUpd.City = update.City;
                workToUpd.Dob = update.Dob;
                workToUpd.Name = update.Name;
                workToUpd.Salary = update.Salary;
                workToUpd.Sex = update.Sex;
                workToUpd.Specialty = update.Specialty;
                workToUpd.Surname = update.Surname;
                workToUpd.ShopId = update.ShopId;

                _context.Update(workToUpd);
            }
            
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> UpdateCustomer(IEnumerable<UpdateCustomer> dto)
    {
        try
        {
            foreach (var update in dto)
            {
                var workToUpd = await _context.Customers.FirstOrDefaultAsync(x => x.CustomerId == update.CustomerId);
                workToUpd.Address = update.Address;
                workToUpd.City = update.City;
                workToUpd.Dob = update.Dob;
                workToUpd.Name = update.Name;
                workToUpd.Phone = update.Phone;
                workToUpd.Sex = update.Sex;
                workToUpd.Zip = update.Zip;
                workToUpd.Surname = update.Surname;
                _context.Update(workToUpd);
            }
            
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }    
    }
    

    public async Task<bool> DeleteWorkerById(Guid id)
    {
        var worker = await GetWorkerById(id);
        if (worker is null)
        {
            return false;
        }

        try
        {
            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> GetMonthStatistic()
    {
        try
        {
            var prod = await _productService.GetPopularProducts();
            var document = new Document();
            var page = new Page();
            document.Pages.Add(page);
            var page2 = new Page();
            document.Pages.Add(page2);
            var page3 = new Page();
            document.Pages.Add(page3);
            var page4 = new Page();
            document.Pages.Add(page4);

            var label = new Label("TOP 10 popular products this month", 0, 50, 300, 30, Font.HelveticaBold, 16,
                TextAlign.Center);
            page.Elements.Add(label);
            var table = new Table2(-40, 100, 600, 1100);

            table.Columns.Add(150);
            table.Columns.Add(100);
            table.Columns.Add(100);
            table.Columns.Add(100);
            table.Columns.Add(50);
            table.Columns.Add(50);
            table.Columns.Add(50);

            Row2 row1 = table.Rows.Add(100, Font.HelveticaBold, 16, Grayscale.Black, Grayscale.Gray);
            row1.CellDefault.Align = TextAlign.Center;
            row1.CellDefault.VAlign = VAlign.Center;
            row1.Cells.Add("Name");
            row1.Cells.Add("Category");
            row1.Cells.Add("Producing Company");
            row1.Cells.Add("Producing Country");
            row1.Cells.Add("Amount on stores");
            row1.Cells.Add("Total orders");
            row1.Cells.Add("Price");
            var i = 1;
            foreach (var productDto in prod)
            {
                table.Rows.Add(70);
                table.Rows[i].Cells.Add(productDto.Name);
                table.Rows[i].Cells.Add(productDto.Category);
                table.Rows[i].Cells.Add(productDto.ProducingCompany);
                table.Rows[i].Cells.Add(productDto.ProducingCountry);
                table.Rows[i].Cells.Add($"{productDto.AmountOnStore}");
                table.Rows[i].Cells.Add($"{productDto.CountOrders}");
                table.Rows[i].Cells.Add($"{productDto.Price}$");
                i++;
            }

            var shops = await _shopService.Get15MostPopularShops();
            var label2 = new Label("TOP 15 popular shops this month", 0, 50, 300, 30, Font.HelveticaBold, 16,
                TextAlign.Center);
            page2.Elements.Add(label2);
            var table2 = new Table2(-40, 100, 600, 1500);

            table2.Columns.Add(150);
            table2.Columns.Add(100);
            table2.Columns.Add(100);
            table2.Columns.Add(100);
            table2.Columns.Add(50);

            Row2 row2 = table2.Rows.Add(100, Font.HelveticaBold, 16, Grayscale.Black, Grayscale.Gray);
            row2.CellDefault.Align = TextAlign.Center;
            row2.CellDefault.VAlign = VAlign.Center;
            row2.Cells.Add("Name");
            row2.Cells.Add("City");
            row2.Cells.Add("Address");
            row2.Cells.Add("Region");
            row2.Cells.Add("Size");
            var j = 1;
            foreach (var shop in shops)
            {
                table2.Rows.Add(70);
                table2.Rows[j].Cells.Add(shop.Name);
                table2.Rows[j].Cells.Add(shop.City);
                table2.Rows[j].Cells.Add(shop.Address);
                table2.Rows[j].Cells.Add(shop.Region);
                table2.Rows[j].Cells.Add(shop.Size);
                j++;
            }

            var shopsCosts = await _shopService.Get15MostExpensiveShop();
            var label3 = new Label("TOP 15 expensive shops this month", 0, 50, 300, 30, Font.HelveticaBold, 16,
                TextAlign.Center);
            page3.Elements.Add(label3);
            var table3 = new Table2(-40, 100, 600, 1500);

            table3.Columns.Add(150);
            table3.Columns.Add(100);
            table3.Columns.Add(100);
            table3.Columns.Add(100);
            table3.Columns.Add(50);
            table3.Columns.Add(50);

            Row2 row3 = table3.Rows.Add(100, Font.HelveticaBold, 16, Grayscale.Black, Grayscale.Gray);
            row3.CellDefault.Align = TextAlign.Center;
            row3.CellDefault.VAlign = VAlign.Center;
            row3.Cells.Add("Name");
            row3.Cells.Add("City");
            row3.Cells.Add("Address");
            row3.Cells.Add("Region");
            row3.Cells.Add("Size");
            row3.Cells.Add("Cost");
            var k = 1;
            foreach (var shop in shopsCosts)
            {
                table3.Rows.Add(70);
                table3.Rows[k].Cells.Add(shop.Shop.Name);
                table3.Rows[k].Cells.Add(shop.Shop.City);
                table3.Rows[k].Cells.Add(shop.Shop.Address);
                table3.Rows[k].Cells.Add(shop.Shop.Region);
                table3.Rows[k].Cells.Add(shop.Shop.Size);
                table3.Rows[k].Cells.Add($"{shop.TotalCost}$");
                k++;
            }

            var popularCities = await _orderService.GetPopularRecipientCities();
            var label4 = new Label("TOP 5 popular cities", 0, 50, 300, 30, Font.HelveticaBold, 16, TextAlign.Center);
            page4.Elements.Add(label4);
            var table4 = new Table2(-40, 100, 300, 500);

            table4.Columns.Add(150);
            table4.Columns.Add(100);

            Row2 row4 = table4.Rows.Add(100, Font.HelveticaBold, 16, Grayscale.Black, Grayscale.Gray);
            row4.CellDefault.Align = TextAlign.Center;
            row4.CellDefault.VAlign = VAlign.Center;
            row4.Cells.Add("City");
            row4.Cells.Add("Orders Count");

            var m = 1;
            foreach (var city in popularCities)
            {
                table4.Rows.Add(60);
                table4.Rows[m].Cells.Add(city.City);
                table4.Rows[m].Cells.Add($"{city.Count}");
                m++;
            }


            page.Elements.Add(table);
            page2.Elements.Add(table2);
            page3.Elements.Add(table3);
            page4.Elements.Add(table4);

            document.Draw($@"{Environment.CurrentDirectory}\MonthStatistic.pdf");
            return true;
        }
        catch(Exception e)
        {
            return false;
        }
    }
}