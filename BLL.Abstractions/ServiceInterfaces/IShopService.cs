using Core.DTO_Models;
using DAL.Models;

namespace BLL.Abstractions.ServiceInterfaces;

public interface IShopService
{
    Task<bool> CreateShop(ShopDto dto);
    Task<bool> DelistShop(Guid id);
    Task<bool> UpdateShopInfo(IEnumerable<ShopDto> dto);
    Task<IEnumerable<Shop>> GetShopsByCity(string city);
    Task<IEnumerable<Shop>> Get15MostPopularShops();
    Task<IEnumerable<ShopCost>> Get15MostExpensiveShop();
    Task<IEnumerable<ShopDropDown>> GetShopDropDowns();
    Task<IEnumerable<string>> GetShopCities();
    Task<Shop> GetShopById(Guid id);
}