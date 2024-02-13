namespace VendorSmart.Shared.Core.Util.Specification.Evaluator;
public interface ISpecificationEvaluator
{
    IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T> specification);
}
