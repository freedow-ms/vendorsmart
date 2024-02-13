using System.Linq.Expressions;

namespace VendorSmart.Shared.Core.Util.Specification;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    List<SpecificationOrderCriteria<T>> OrderCriteria { get; }
}
