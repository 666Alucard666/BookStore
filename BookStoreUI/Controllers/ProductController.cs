using BLL.Abstractions.ServiceInterfaces;
using Core.DTO_Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreUI.Controllers;

[ApiController]
[Route("api/product")]
[Produces("application/json")]
public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpPost("CreateProduct")]
    public async Task<ActionResult<bool>>CreateProduct([FromBody]IEnumerable<ProductDto> productDtos)
    {
        var res = new List<(bool, string)>();
        foreach (var productDto in productDtos)
        {
            res.Add((await _productService.CreateProduct(productDto), productDto.Name));
        }
        if (res.All(x => x.Item1))
        {
            return Ok(res);
        }
        
        return BadRequest($"Failed to create products:{string.Join(';',res.Select(x => x.Item2))}");
    }
    
    [HttpPut("DeleteProduct")]
    public async Task<ActionResult<bool>>DeleteProduct([FromBody]IEnumerable<DeleteProductRequest>prodToDelete)
    {
        var res = new List<(bool, string)>();
        res.Add((await _productService.DeleteProduct(prodToDelete), prodToDelete.First().ProductId.ToString()));
        if (res.All(x => x.Item1))
        {
            return Ok(res);
        }
        
        return BadRequest($"Failed to create products:{string.Join(';',res.Select(x => x.Item2))}");
    }
    
    [HttpPut("EditProductInfo")]
    public async Task<ActionResult<bool>>EditProductInfo([FromBody]IEnumerable<ProductDto> productDtos)
    {
        var res = new List<(bool, string)>();
        foreach (var productDto in productDtos)
        {
            res.Add((await _productService.EditProductInfo(productDto), productDto.Name));
        }
        if (res.All(x => x.Item1))
        {
            return Ok(res);
        }
        
        return BadRequest($"Failed to create products:{string.Join(';',res.Select(x => x.Item2))}");
    }
    
    [HttpGet("GetProducts")]
    public async Task<ActionResult<IEnumerable<ProductDto>>>GetProducts([FromQuery]string? category)
    {
        var res = await _productService.GetProducts(category);
        return Ok(res);
    }
    
    [HttpGet("GetProductById")]
    public async Task<ActionResult<ProductDto>>GetProductById([FromQuery]Guid productId)
    {
        var res = await _productService.GetProductById(productId);
        return Ok(res);
    }

    [HttpGet("GetPopularProducts")]
    public async Task<ActionResult<IEnumerable<ProductDto>>>GetPopularProducts()
    {
        var res = await _productService.GetPopularProducts();
        return Ok(res);
    }
}