using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using System.Runtime.CompilerServices;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<ProductBrand> _productBrandRepository;
        private readonly IRepository<ProductType> _productTypesRepository;

        public ProductsController(IRepository<Product> productsRepository,IRepository<ProductBrand> productBrandRepository,IRepository<ProductType> productTypesRepository)
        {
            _productsRepository = productsRepository;
            _productBrandRepository = productBrandRepository;
            _productTypesRepository = productTypesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        {
            var products = await _productsRepository.ListAsync(x=>true, x=>x.ProductBrand, x=>x.ProductType);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productsRepository.GetByIdAsync(id, x=>x.ProductBrand, x=>x.ProductType);
            return Ok(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var brands = await _productBrandRepository.ListAllAsync();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var types = await _productTypesRepository.ListAllAsync();
            return Ok(types);
        }
    }
}