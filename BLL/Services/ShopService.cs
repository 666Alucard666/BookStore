using BLL.Abstractions.ServiceInterfaces;
using Core.DTO_Models;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class ShopService: IShopService
{
    private readonly GigienaStoreDbContext _context;

    public ShopService(GigienaStoreDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> CreateShop(ShopDto dto)
    {
        try
        {
            if (dto is null)
            {
                return false;
            }

            var shop = new Shop
            {
                ShopId = Guid.NewGuid(),
                Address = dto.Address,
                City = dto.City,
                EndWorkingHours = new TimeSpan((long) (dto.EndWorkingHours*36000000000)),
                Name = dto.Name,
                Size = dto.Size,
                Region = dto.Region,
                StartWorkingHours = new TimeSpan((long) (dto.StartWorkingHours*36000000000))
            };

            await _context.Shops.AddAsync(shop);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }

    }

    public async Task<bool> DelistShop(Guid id)
    {
        try
        {
            var shop = await _context.Shops.FirstOrDefaultAsync(x => x.ShopId == id);
            if (shop is null)
            {
                return false;
            }

            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> UpdateShopInfo(IEnumerable<ShopDto> dto)
    {
        try
        {
            foreach (var update in dto)
            {
                var shopToUpd = await _context.Shops.FirstOrDefaultAsync(x => x.ShopId == update.ShopId.Value);
                shopToUpd.Address = update.Address;
                shopToUpd.City = update.City;
                shopToUpd.Name = update.Name;
                shopToUpd.Region = update.Region;
                shopToUpd.Size = update.Size;
                shopToUpd.EndWorkingHours = new TimeSpan((long) (update.EndWorkingHours*36000000000));
                shopToUpd.StartWorkingHours = new TimeSpan((long) (update.StartWorkingHours*36000000000));
                _context.Update(shopToUpd);
            }
            
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<IEnumerable<Shop>> GetShopsByCity(string city)
    {
        return await _context.Shops.Include(x => x.Workers).Include(x => x.ShopProducts).Include(x => x.Orders).Where(x => x.City == city).ToListAsync();
    }

    public async Task<IEnumerable<Shop>> Get15MostPopularShops()
    {
        var grop = await _context.Orders.GroupBy(x => x.ShopId).Select(x => new
        {
            ShopId = x.Key, Count = x.Count()
        }).Where(x => x.ShopId.HasValue && x.ShopId != new Guid()).OrderByDescending(x => x.Count).Take(15).ToListAsync();
        
        return await GetShopsByIds(grop.Select(x => x.ShopId.Value)) ;
    }

    public async Task<IEnumerable<ShopCost>> Get15MostExpensiveShop()
    {
        var group = await _context.Workers.GroupBy(x => x.ShopId).Select(x => new
        {
            ShopId = x.Key,
            Sum = x.Select(x => x.Salary).Sum()
        }).OrderByDescending(x => x.Sum).Take(15).ToListAsync();
        var shops = await GetShopsByIds(group.Select(x => x.ShopId.Value));
        return shops.Select(x => new ShopCost
        {
            Shop = x,
            TotalCost = group.First(g => g.ShopId == x.ShopId).Sum
        });
    }

    public async Task<IEnumerable<ShopDropDown>> GetShopDropDowns()
    {
        return await _context.Shops.Select(x => new ShopDropDown
        {
            ShopId = x.ShopId,
            Name = x.Name
        }).ToListAsync();
    }

    public async Task<IEnumerable<string>> GetShopCities()
    {
        return await _context.Shops.AsNoTracking().Select(x => x.City).Distinct().ToListAsync();
    }

    public async Task<Shop> GetShopById(Guid id)
    {
        return await _context.Shops.AsNoTracking().Include(x => x.Orders).FirstOrDefaultAsync(x => x.ShopId == id);
    }

    private async Task<IEnumerable<Shop>> GetShopsByIds(IEnumerable<Guid> ids)
    {
        var result = new List<Shop>(ids.Count());
        foreach (var id in ids)
        {
            result.Add(await _context.Shops.Include(x => x.Workers).FirstOrDefaultAsync(x => x.ShopId == id));
        }

        return result;
    }
}