using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using AutoMapper;
using API.DTO;
using System.Linq.Expressions;
using API.Base.Controllers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductGridController : BasePaginableController<Product>
    {
        public ProductGridController(IRepository<Product> productsRepository, IMapper mapper) : base(productsRepository)
        {
        }
        
        protected override Expression<Func<Product, object>>[] GetIncludes()
        {
            // Placeholder iplementation for GetIncludes
            return new Expression<Func<Product, object>>[] { p => p.ProductBrand, p=>p.ProductType };
        }
    }
}