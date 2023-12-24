using System.Linq.Expressions;
using System.Reflection;
using API.DTO;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Base.Controllers
{
    [ApiController]
    public abstract class BasePaginableController<T,V> : ControllerBase where T : BaseEntity where V: class
    {
        private readonly IRepository<T> _repository;
        private readonly IMapper _mapper;

        protected BasePaginableController(IRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET api/[controller]
        public async Task<ActionResult<V>> Get(int page = 1, int pageSize = 10, string orderBy = "Id", string sortOrder = "asc",  string filterProperty = null, string filterValue = null)
        {
            try
            {
                Expression<Func<T, object>> orderByExpression = GetOrderByExpression(orderBy);
                Expression<Func<T, bool>> filterExpression = GetFilterExpression(filterProperty, filterValue);

                var items = await _repository.ListPaginatedAsync(
                    criteria: filterExpression,
                    orderBy: orderByExpression,
                    page: page,
                    pageSize: pageSize,
                    sortOrder: sortOrder,
                    includes: GetIncludes()
                );

                var data = _mapper.Map<IReadOnlyList<V>>(items);
                var totalItems = await _repository.CountAsync(filterExpression);

                return Ok(new Pagination<V>(page,pageSize, totalItems, data));
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (log, return a specific status code, etc.)
                throw ex;
            }
        }

        protected virtual Expression<Func<T, object>>[] GetIncludes()
        {
            return new Expression<Func<T, object>>[] { };
        }

        private Expression<Func<T, object>> GetOrderByExpression(string orderBy)
        {
            PropertyInfo property = typeof(T).GetProperty(orderBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            
            if (property == null)
            {
                // Default to Id if the provided property does not exist
                property = typeof(T).GetProperty("Id");
            }

            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
            UnaryExpression unaryExpression = Expression.Convert(propertyAccess, typeof(object));
            return Expression.Lambda<Func<T, object>>(unaryExpression, parameter);
        }

        private Expression<Func<T, bool>> GetFilterExpression(string filterProperty, string filterValue)
        {
            if (filterProperty == null || filterValue == null)
            {
                return t => true; // No filtering if the provided property does not exist
            }

            PropertyInfo property = typeof(T).GetProperty(filterProperty, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null)
            {
                return t => true; // No filtering if the provided property does not exist
            }

            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);

            // Check if the property type is string
            if (property.PropertyType == typeof(string))
            {
                MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                ConstantExpression constantValue = Expression.Constant(filterValue, typeof(string));
                MethodCallExpression containsExpression = Expression.Call(propertyAccess, containsMethod, constantValue);
                return Expression.Lambda<Func<T, bool>>(containsExpression, parameter);
            }

            // For non-string properties, use Equality
            ConstantExpression equalValue = Expression.Constant(Convert.ChangeType(filterValue, property.PropertyType));
            BinaryExpression equalExpression = Expression.Equal(propertyAccess, equalValue);
            return Expression.Lambda<Func<T, bool>>(equalExpression, parameter);
        }
    }
}