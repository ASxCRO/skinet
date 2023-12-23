using System.Linq.Expressions;

namespace Core.Specifications
{
    public class Specification<T> : ISpecification<T>
    {
        public Specification(Expression<Func<T,bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria{get;}
        public List<Expression<Func<T, object>>> Includes {get;} 
            = new();

        public Specification<T> AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
            return this;
        }
    }
}