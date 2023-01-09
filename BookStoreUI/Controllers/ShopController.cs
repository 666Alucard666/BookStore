using BLL.Abstractions.ServiceInterfaces;
using Core.DTO_Models;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreUI.Controllers;

[ApiController]
[Route("api/shop")]
[Produces("application/json")]
public class ShopController : Controller
{
    private readonly IShopService _shopService;

    public ShopController(IShopService shopService)
    {
        _shopService = shopService;
    }
    
    [HttpPost("CreateShop")]
    public async Task<ActionResult<bool>>CreateShop([FromBody]IEnumerable<ShopDto> shopDtos)
    {
        var res = new List<(bool, string)>();
        foreach (var productDto in shopDtos)
        {
            res.Add((await _shopService.CreateShop(productDto), productDto.Name));
        }
        if (res.All(x => x.Item1))
        {
            return Ok(res);
        }
        
        return BadRequest($"Failed to create shops:{string.Join(';',res.Select(x => x.Item2))}");
    }
    
    [HttpPut("DelistShops")]
    public async Task<ActionResult<bool>>DelistShops([FromBody]IEnumerable<Guid> shopDtos)
    {
        var res = new List<(bool, string)>();
        foreach (var productDto in shopDtos)
        {
            res.Add((await _shopService.DelistShop(productDto), productDto.ToString()));
        }
        if (res.All(x => x.Item1))
        {
            return Ok(res);
        }
        
        return BadRequest($"Failed to create shops:{string.Join(';',res.Select(x => x.Item2))}");
    }
    
    [HttpPut("UpdateShopsInfo")]
    public async Task<ActionResult<bool>>UpdateShopsInfo([FromBody]IEnumerable<ShopDto> shopDtos)
    {
        var res = await _shopService.UpdateShopInfo(shopDtos);
        if (res)
        {
            return Ok(res);
        }
        return BadRequest("Failed to update shops");
    }
    
    [HttpGet("GetShopsByCity")]
    public async Task<ActionResult<IEnumerable<Shop>>>GetShopsByCity([FromQuery] string city)
    {
        var res = await _shopService.GetShopsByCity(city);
        if (res.Any())
        {
            return Ok(res);
        }
        return BadRequest($"Failed to get shops by {city}");
    }    
    
    [HttpGet("GetShopById")]
    public async Task<ActionResult<IEnumerable<Shop>>>GetShopById([FromQuery] Guid id)
    {
        var res = await _shopService.GetShopById(id);
        if (res is not null)
        {
            return Ok(res);
        }
        return BadRequest($"Failed to get shops by {id}");
    }   
    
    [HttpGet("GetShopsDropDown")]
    public async Task<ActionResult<IEnumerable<ShopDropDown>>>GetShopsDropDown()
    {
        var res = await _shopService.GetShopDropDowns();
        if (res.Any())
        {
            return Ok(res);
        }
        return BadRequest($"Failed to get shops ");
    }    
    
    [HttpGet("Get15MostPopularShops")]
    public async Task<ActionResult<IEnumerable<Shop>>>Get15MostPopularShops()
    {
        var res = await _shopService.Get15MostPopularShops();
        if (res.Any())
        {
            return Ok(res);
        }
        return BadRequest($"Failed to get shops");
    }

    [HttpGet("Get15MostExpensiveShop")]
    public async Task<ActionResult<IEnumerable<ShopCost>>>Get15MostExpensiveShop()
    {
        var res = await _shopService.Get15MostExpensiveShop();
        if (res.Any())
        {
            return Ok(res);
        }
        return BadRequest($"Failed to get shops");
    }

    [HttpGet("GetCities")]
    public async Task<ActionResult<IEnumerable<string>>> GetCities()
    {
        var res = await _shopService.GetShopCities();
        if (res.Any())
        {
            return Ok(res);
        }
        return BadRequest($"Failed to get cities");
    }
}