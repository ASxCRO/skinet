using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _storeContext;
        public ProductRepository(StoreContext storeContext)
        {
            this._storeContext = storeContext;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _storeContext.Products
            .Include(x=>x.ProductType)
            .Include(x=>x.ProductBrand)
            .FirstAsync(x=>x.Id == id);
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
             var products = await _storeContext
             .Products
                .Include(x=>x.ProductType)
                .Include(x=>x.ProductBrand)
                .ToListAsync();
            return products;
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            var types = await _storeContext.ProductTypes
                .ToListAsync();
            return types;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
           var brands = await _storeContext.ProductBrands
                .ToListAsync();
            return brands;
        }

    }
}