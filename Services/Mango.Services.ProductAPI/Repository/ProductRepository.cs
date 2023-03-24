using AutoMapper;
using Mango.Services.ProductAPI.DbContexts;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private IMapper _mapper;
        public ProductRepository(ApplicationDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper=mapper;
        }
        public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
            Product product=_mapper.Map<ProductDto, Product>(productDto);
            if (productDto.ProductId>0)
            {
                _dbContext.Products.Update(product);
            }
            else
            {
				_dbContext.Add(product);
			}
			await _dbContext.SaveChangesAsync();
			return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            Product product = await _dbContext.Products.FirstOrDefaultAsync(i => i.ProductId == productId);
            if (product==null)
            {
                return false;
            }
            _dbContext.Remove(product);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            Product product = await _dbContext.Products.Where(i=>i.ProductId==productId).FirstOrDefaultAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            List<Product> productList = await _dbContext.Products.ToListAsync();
            return _mapper.Map<List<ProductDto>>(productList);
        }
    }
}
