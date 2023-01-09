using BLL.Abstractions.ServiceInterfaces;
using Core.DTO_Models;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class ProductService : IProductService
{
    private readonly GigienaStoreDbContext _context;

    public ProductService(GigienaStoreDbContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateProduct(ProductDto productDto)
    {
        try
        {
            var prodId = Guid.NewGuid();
            await _context.Products.AddAsync(new Product
            {
                ProductId = prodId,
                Image = productDto.Image,
                Name = productDto.Name,
                Price = productDto.Price,
                ProducingCompany = productDto.ProducingCompany,
                ProducingCountry = productDto.ProducingCountry,
                ProducingDate = productDto.ProducingDate
            });
            await _context.ProductInfos.AddAsync(new ProductInfo
            {
                ProductInfoId = prodId,
                Capacity = productDto.Capacity,
                Category = productDto.Category,
                Contraindication = productDto.Contraindication,
                Gender = productDto.Gender,
                Instruction = productDto.Instruction
            });
            await _context.ShopProducts.AddRangeAsync(productDto.ShopProducts.Select(x => new ShopProduct
            {
                ShopId = x.ShopId,
                ProductId = prodId,
                Count = x.Count
            }));
            
            if (productDto.SkinCareProduct is not null)
            {
                await _context.SkinCareProducts.AddAsync(new SkinCareProduct
                {
                    SkinCareProductId = prodId,
                    SkinType = productDto.SkinCareProduct.SkinType,
                    UsePurpose = productDto.SkinCareProduct.UsePurpose,
                    AgeRestrictionsStart = productDto.SkinCareProduct.AgeRestrictionsStart,
                    AgeRestrictionsEnd = productDto.SkinCareProduct.AgeRestrictionsEnd,
                });
            }
            
            if (productDto.HairCareProduct is not null)
            {
                await _context.HairCareProducts.AddAsync(new HairCareProduct
                {
                    HairCareProductId = prodId,
                    HairDisease = productDto.HairCareProduct.HairDisease,
                    HairType = productDto.HairCareProduct.HairType,
                    NotContains = productDto.HairCareProduct.NotContains,
                    IsAntiDandruff = productDto.HairCareProduct.IsAntiDandruff
                });
            }
            
            if (productDto.NailsCareProduct is not null)
            {
                await _context.NailsCareProducts.AddAsync(new NailsCareProduct
                {
                    NailsCareProductId = prodId,
                    NailsDisease = productDto.NailsCareProduct.NailsDisease,
                    NailsType = productDto.NailsCareProduct.NailsType,
                    Fragrance = productDto.NailsCareProduct.Fragrance,
                });
            }
            
            if (productDto.OralCavityProduct is not null)
            {
                await _context.OralCavityProducts.AddAsync(new OralCavityProduct
                {
                    OralCavityProductId = prodId,
                    GumDiseaseType = productDto.OralCavityProduct.GumDiseaseType,
                    IsWhitening = productDto.OralCavityProduct.IsWhitening,
                    IsHerbalBase = productDto.OralCavityProduct.IsHerbalBase,
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
        
    }
    

    public async Task<bool> DeleteProduct(IEnumerable<DeleteProductRequest> request)
    {
        try
        {
            var product = await _context.Products.Include(x => x.ShopProducts).Include(x => x.ProductInfo)
                .ThenInclude(x => x.HairCareProduct)
                .Include(x => x.ProductInfo)
                .ThenInclude(x => x.OralCavityProduct)
                .Include(x => x.ProductInfo)
                .ThenInclude(x => x.NailsCareProduct)
                .Include(x => x.ProductInfo)
                .ThenInclude(x => x.SkinCareProduct).FirstOrDefaultAsync(x => x.ProductId == request.First().ProductId);
            var prod = product.ProductInfo;
            foreach (var productRequest in request)
            {
                product.ShopProducts.Remove(
                    product.ShopProducts.FirstOrDefault(x => x.ShopId == productRequest.ShopId && x.ProductId == productRequest.ProductId));
                var sh = await _context.Shops.Include(x => x.ShopProducts).FirstOrDefaultAsync(x => x.ShopId == productRequest.ShopId);
                sh.ShopProducts.Remove(sh.ShopProducts.FirstOrDefault(x =>
                    x.ShopId == productRequest.ShopId && x.ProductId == productRequest.ProductId));
                product.OrderProducts.Remove(
                    product.OrderProducts.FirstOrDefault(x => x.ProductId == productRequest.ProductId));
            }
            
            switch (prod.Category)
            {
                case "Nails":
                    _context.Remove(prod.NailsCareProduct);
                    break;
                case "Skin":
                    _context.Remove(prod.SkinCareProduct);
                    break;
                case "Oral":
                    _context.Remove(prod.OralCavityProduct);
                    break;
                case "Hair":
                    _context.Remove(prod.HairCareProduct);
                    break;
                default:
                    break;
            }
            _context.Remove(prod);
            _context.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> EditProductInfo(ProductDto productDto)
    {
        try
        {
            var product = await _context.Products
                .Include(x => x.ProductInfo)
                .ThenInclude(x => x.HairCareProduct)
                .Include(x => x.ProductInfo)
                .ThenInclude(x => x.OralCavityProduct)
                .Include(x => x.ProductInfo)
                .ThenInclude(x => x.NailsCareProduct)
                .Include(x => x.ProductInfo)
                .ThenInclude(x => x.SkinCareProduct).FirstOrDefaultAsync(x => x.ProductId == productDto.ProductId);
            var prod = product.ProductInfo;

            product.Image = productDto.Image;
            product.Name = productDto.Name;
            product.ProducingCountry = productDto.ProducingCountry;
            product.ProducingDate = productDto.ProducingDate;
            product.Price = productDto.Price;

            var prods = productDto.ShopProducts.Select(x => new ShopProduct
            {
                Count = x.Count,
                ProductId = x.ProductId.Value,
                ShopId = x.ShopId
            }).ToList();
            var prodToUpdate = new List<ShopProduct>();
            var prodToCreate = new List<ShopProduct>();
            foreach (var shopProduct in prods)
            {
                if (!_context.ShopProducts.AsNoTracking().Any(x => shopProduct.ProductId == x.ProductId && shopProduct.ShopId == x.ShopId))
                {
                    prodToCreate.Add(shopProduct);
                }
                else
                {
                    prodToUpdate.Add(shopProduct);
                }
            }
            
            prod.Capacity = productDto.Capacity;
            prod.Contraindication = productDto.Contraindication;
            prod.Instruction = productDto.Instruction;
            prod.Gender = productDto.Gender;

            switch (prod.Category)
            {
                case "Nails":
                    prod.NailsCareProduct.Fragrance = productDto.NailsCareProduct.Fragrance;
                    prod.NailsCareProduct.NailsDisease = productDto.NailsCareProduct.NailsDisease;
                    prod.NailsCareProduct.NailsType = productDto.NailsCareProduct.NailsType;
                    _context.Update(prod.NailsCareProduct);
                    break;
                case "Skin":
                    prod.SkinCareProduct.SkinType = productDto.SkinCareProduct.SkinType;
                    prod.SkinCareProduct.UsePurpose = productDto.SkinCareProduct.UsePurpose;
                    prod.SkinCareProduct.AgeRestrictionsEnd = productDto.SkinCareProduct.AgeRestrictionsEnd;
                    prod.SkinCareProduct.AgeRestrictionsStart = productDto.SkinCareProduct.AgeRestrictionsStart;
                    _context.Update(prod.SkinCareProduct);
                    break;
                case "Oral":
                    prod.OralCavityProduct.IsWhitening = productDto.OralCavityProduct.IsWhitening;
                    prod.OralCavityProduct.GumDiseaseType = productDto.OralCavityProduct.GumDiseaseType;
                    prod.OralCavityProduct.IsHerbalBase = productDto.OralCavityProduct.IsHerbalBase;
                    _context.Update(prod.OralCavityProduct);
                    break;
                case "Hair":
                    prod.HairCareProduct.HairDisease = productDto.HairCareProduct.HairDisease;
                    prod.HairCareProduct.HairType = productDto.HairCareProduct.HairType;
                    prod.HairCareProduct.NotContains = productDto.HairCareProduct.NotContains;
                    prod.HairCareProduct.IsAntiDandruff = productDto.HairCareProduct.IsAntiDandruff;
                    _context.Update(prod.HairCareProduct);
                    break;
                default:
                    break;
            }

            _context.Update(prod);
            _context.Update(product);
            _context.UpdateRange(prodToUpdate);
            await _context.AddRangeAsync(prodToCreate);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<IEnumerable<ProductDto>> GetProducts(string? category)
    {
        var products = await _context.Products.AsNoTracking().Include(x => x.ShopProducts)
            .Include(x => x.ProductInfo)
            .ThenInclude(x => x.HairCareProduct)
            .Include(x => x.ProductInfo)
            .ThenInclude(x => x.OralCavityProduct)
            .Include(x => x.ProductInfo)
            .ThenInclude(x => x.NailsCareProduct)
            .Include(x => x.ProductInfo)
            .ThenInclude(x => x.SkinCareProduct)
            .ToListAsync();
        if (category is not null)
        {
            products = products.Where(x => x.ProductInfo.Category == category).ToList();
        }

        var res = products.Select(x => new ProductDto
        {
            ProductId = x.ProductId,
            Name = x.Name,
            ProducingCountry = x.ProducingCountry,
            Price = x.Price,
            ProducingDate = x.ProducingDate,
            ProducingCompany = x.ProducingCompany,
            Image = x.Image,
            Instruction = x.ProductInfo.Instruction,
            Capacity = x.ProductInfo.Capacity,
            Contraindication = x.ProductInfo.Contraindication,
            Category = x.ProductInfo.Category,
            Gender = x.ProductInfo.Gender,
            ShopProducts = x.ShopProducts.Select(x => new ShopProductsDto
            {
                ProductId = x.ProductId,
                ShopId = x.ShopId,
                Count = x.Count
            }),
            AmountOnStore = x.ShopProducts.Select(x => x.Count).Sum(),
            HairCareProduct = x.ProductInfo.Category.Equals("Hair") ? new HairDto
            {
                HairType = x.ProductInfo.HairCareProduct.HairType,
                HairDisease = x.ProductInfo.HairCareProduct.HairDisease,
                IsAntiDandruff = x.ProductInfo.HairCareProduct.IsAntiDandruff,
                NotContains = x.ProductInfo.HairCareProduct.NotContains
            } : null,
            NailsCareProduct = x.ProductInfo.Category.Equals("Nails") ? new NailsDto
            {
                NailsType = x.ProductInfo.NailsCareProduct.NailsType,
                NailsDisease = x.ProductInfo.NailsCareProduct.NailsDisease,
                Fragrance = x.ProductInfo.NailsCareProduct.Fragrance
            } : null,
            OralCavityProduct = x.ProductInfo.Category.Equals("Oral") ? new OralCavityDto
            {
                GumDiseaseType = x.ProductInfo.OralCavityProduct.GumDiseaseType,
                IsWhitening = x.ProductInfo.OralCavityProduct.IsWhitening,
                IsHerbalBase = x.ProductInfo.OralCavityProduct.IsHerbalBase
            } : null,
            SkinCareProduct = x.ProductInfo.Category.Equals("Skin") ? new SkinCareDto
            {
                SkinType = x.ProductInfo.SkinCareProduct.SkinType,
                UsePurpose = x.ProductInfo.SkinCareProduct.UsePurpose,
                AgeRestrictionsStart = x.ProductInfo.SkinCareProduct.AgeRestrictionsStart,
                AgeRestrictionsEnd = x.ProductInfo.SkinCareProduct.AgeRestrictionsEnd
            } : null
        }).ToList();
        return res;
    }

    public async Task<ProductDto> GetProductById(Guid id)
    {
        var product = await _context.Products.AsNoTracking().Include(x => x.ShopProducts)
            .Include(x => x.ProductInfo)
            .ThenInclude(x => x.HairCareProduct)
            .Include(x => x.ProductInfo)
            .ThenInclude(x => x.OralCavityProduct)
            .Include(x => x.ProductInfo)
            .ThenInclude(x => x.NailsCareProduct)
            .Include(x => x.ProductInfo)
            .ThenInclude(x => x.SkinCareProduct)
            .FirstOrDefaultAsync(x => x.ProductId == id);
        var res = new ProductDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            ProducingCountry = product.ProducingCountry,
            Price = product.Price,
            ProducingDate = product.ProducingDate,
            ProducingCompany = product.ProducingCompany,
            Image = product.Image,
            Instruction = product.ProductInfo.Instruction,
            Capacity = product.ProductInfo.Capacity,
            Contraindication = product.ProductInfo.Contraindication,
            Category = product.ProductInfo.Category,
            Gender = product.ProductInfo.Gender,
            ShopProducts = product.ShopProducts.Select(x => new ShopProductsDto
            {
                ProductId = product.ProductId,
                ShopId = x.ShopId,
                Count = x.Count
            }),
            AmountOnStore = product.ShopProducts.Select(x => x.Count).Sum(),
            HairCareProduct = product.ProductInfo.Category.Equals("Hair")
                ? new HairDto
                {
                    HairType = product.ProductInfo.HairCareProduct.HairType,
                    HairDisease = product.ProductInfo.HairCareProduct.HairDisease,
                    IsAntiDandruff = product.ProductInfo.HairCareProduct.IsAntiDandruff,
                    NotContains = product.ProductInfo.HairCareProduct.NotContains
                }
                : null,
            NailsCareProduct = product.ProductInfo.Category.Equals("Nails")
                ? new NailsDto
                {
                    NailsType = product.ProductInfo.NailsCareProduct.NailsType,
                    NailsDisease = product.ProductInfo.NailsCareProduct.NailsDisease,
                    Fragrance = product.ProductInfo.NailsCareProduct.Fragrance
                }
                : null,
            OralCavityProduct = product.ProductInfo.Category.Equals("Oral")
                ? new OralCavityDto
                {
                    GumDiseaseType = product.ProductInfo.OralCavityProduct.GumDiseaseType,
                    IsWhitening = product.ProductInfo.OralCavityProduct.IsWhitening,
                    IsHerbalBase = product.ProductInfo.OralCavityProduct.IsHerbalBase
                }
                : null,
            SkinCareProduct = product.ProductInfo.Category.Equals("Skin")
                ? new SkinCareDto
                {
                    SkinType = product.ProductInfo.SkinCareProduct.SkinType,
                    UsePurpose = product.ProductInfo.SkinCareProduct.UsePurpose,
                    AgeRestrictionsStart = product.ProductInfo.SkinCareProduct.AgeRestrictionsStart,
                    AgeRestrictionsEnd = product.ProductInfo.SkinCareProduct.AgeRestrictionsEnd
                }
                : null
        };
        return res;
    }

    public async Task<IEnumerable<ProductDto>> GetPopularProducts()
    {
        var productsId = await _context.OrderProducts.AsNoTracking().Include(o => o.Order).Where(x => x.Order.ProcessedDate.Month == DateTime.Now.Month).GroupBy(x => x.ProductId).Select(x => new
        {
            ProductId = x.Key, Count = x.Select(x => x.OrderId).Count()
        }).OrderByDescending(x => x.Count).Take(10).ToListAsync();
        var products = await _context.Products.AsNoTracking().Include(x => x.ShopProducts)
            .Include(x => x.ProductInfo)
            .ThenInclude(x => x.HairCareProduct)
            .Include(x => x.ProductInfo)
            .ThenInclude(x => x.OralCavityProduct)
            .Include(x => x.ProductInfo)
            .ThenInclude(x => x.NailsCareProduct)
            .Include(x => x.ProductInfo)
            .ThenInclude(x => x.SkinCareProduct)
            .Where(x => productsId.Select(x => x.ProductId).Contains(x.ProductId))
            .ToListAsync();
        var res = products.Select(x => new ProductDto
        {
            ProductId = x.ProductId,
            Name = x.Name,
            ProducingCountry = x.ProducingCountry,
            Price = x.Price,
            ProducingDate = x.ProducingDate,
            ProducingCompany = x.ProducingCompany,
            Image = x.Image,
            Instruction = x.ProductInfo.Instruction,
            Capacity = x.ProductInfo.Capacity,
            Contraindication = x.ProductInfo.Contraindication,
            Category = x.ProductInfo.Category,
            Gender = x.ProductInfo.Gender,
            CountOrders = productsId.Where(a => a.ProductId == x.ProductId).Select(x => x.Count).Sum(),
            AmountOnStore = x.ShopProducts.Select(x => x.Count).Sum(),
            ShopProducts = x.ShopProducts.Select(x => new ShopProductsDto
            {
                ProductId = x.ProductId,
                ShopId = x.ShopId,
                Count = x.Count
            }),
            HairCareProduct = x.ProductInfo.Category.Equals("Hair") ? new HairDto
            {
                HairType = x.ProductInfo.HairCareProduct.HairType,
                HairDisease = x.ProductInfo.HairCareProduct.HairDisease,
                IsAntiDandruff = x.ProductInfo.HairCareProduct.IsAntiDandruff,
                NotContains = x.ProductInfo.HairCareProduct.NotContains
            } : null,
            NailsCareProduct = x.ProductInfo.Category.Equals("Nails") ? new NailsDto
            {
                NailsType = x.ProductInfo.NailsCareProduct.NailsType,
                NailsDisease = x.ProductInfo.NailsCareProduct.NailsDisease,
                Fragrance = x.ProductInfo.NailsCareProduct.Fragrance
            } : null,
            OralCavityProduct = x.ProductInfo.Category.Equals("Oral") ? new OralCavityDto
            {
                GumDiseaseType = x.ProductInfo.OralCavityProduct.GumDiseaseType,
                IsWhitening = x.ProductInfo.OralCavityProduct.IsWhitening,
                IsHerbalBase = x.ProductInfo.OralCavityProduct.IsHerbalBase
            } : null,
            SkinCareProduct = x.ProductInfo.Category.Equals("Skin") ? new SkinCareDto
            {
                SkinType = x.ProductInfo.SkinCareProduct.SkinType,
                UsePurpose = x.ProductInfo.SkinCareProduct.UsePurpose,
                AgeRestrictionsStart = x.ProductInfo.SkinCareProduct.AgeRestrictionsStart,
                AgeRestrictionsEnd = x.ProductInfo.SkinCareProduct.AgeRestrictionsEnd
            } : null
        }).ToList();
        return res;
    }
}