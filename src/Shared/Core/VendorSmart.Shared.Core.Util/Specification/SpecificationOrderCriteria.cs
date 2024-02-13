using System.Linq.Expressions;

namespace VendorSmart.Shared.Core.Util.Specification;


public class SpecificationOrderCriteria<T>(Expression<Func<T, object>> keySelector, bool isAscending = true)
{
    public Expression<Func<T, object>> KeySelector { get; private set; } = keySelector;
    public bool IsAscending { get; private set; } = isAscending;
}