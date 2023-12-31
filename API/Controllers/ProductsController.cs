using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using AutoMapper;
using API.DTO;
using API.Helpers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<ProductBrand> _productBrandRepository;
        private readonly IRepository<ProductType> _productTypesRepository;
        private readonly IMapper _mapper;

        public ProductsController(IRepository<Product> productsRepository,
        IRepository<ProductBrand> productBrandRepository,
        IRepository<ProductType> productTypesRepository,
        IMapper mapper)
        {
            _productsRepository = productsRepository;
            _productBrandRepository = productBrandRepository;
            _productTypesRepository = productTypesRepository;
            _mapper = mapper;
        }

        [Cached(600)]
         [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
        {
            var product = await _productsRepository.GetByIdAsync(id, x=>x.ProductBrand, x=>x.ProductType);
            return Ok(_mapper.Map<Product,ProductToReturnDTO>(product));
        }

        [Cached(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var brands = await _productBrandRepository.ListAllAsync();
            return Ok(brands);
        }

        [Cached(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var types = await _productTypesRepository.ListAllAsync();
            return Ok(types);
        }
    }
}