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
            var product = await _storeContext.Products.FindAsync(id);
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
             var products = await _storeContext.Products.ToListAsync();
            return products;
        }
    }
}