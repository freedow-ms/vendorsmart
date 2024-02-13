namespace VendorSmart.Shared.Core.Util.Specification.Evaluator;
public class SpecificationEvaluator : ISpecificationEvaluator
{
    public IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification)
    {
        var query = inputQuery;

        if (specification.Criteria != null)
            query = query.Where(specification.Criteria);

        bool firstOrderApplied = false;
        foreach (var orderCriteria in specification.OrderCriteria)
        {
            if (firstOrderApplied)
            {
                query = orderCriteria.IsAscending
                    ? ((IOrderedQueryable<T>)query).ThenBy(orderCriteria.KeySelector)
                    : ((IOrderedQueryable<T>)query).ThenByDescending(orderCriteria.KeySelector);
            }
            else
            {
                query = orderCriteria.IsAscending
                    ? query.OrderBy(orderCriteria.KeySelector)
                    : query.OrderByDescending(orderCriteria.KeySelector);
                firstOrderApplied = true;
            }
        }

        return query;
    }
}
