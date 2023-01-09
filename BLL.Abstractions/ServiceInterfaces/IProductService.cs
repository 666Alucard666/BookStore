using Core.DTO_Models;
using DAL.Models;

namespace BLL.Abstractions.ServiceInterfaces;

public interface IProductService
{
    Task<bool> CreateProduct(ProductDto productDto);
    Task<bool> DeleteProduct(IEnumerable<DeleteProductRequest> request);
    Task<bool> EditProductInfo(ProductDto productDto);
    Task<IEnumerable<ProductDto>> GetProducts(string? category);
    Task<ProductDto> GetProductById(Guid id);
    Task<IEnumerable<ProductDto>> GetPopularProducts();
}