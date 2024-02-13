using System.Linq.Expressions;
using VendorSmart.Shared.Core.Util.Specification;
using VendorSmart.Vendor.Domain.Entities;

namespace VendorSmart.Vendor.Domain.Specifications;

public class PotentialVendorsSpecification : ISpecification<VendorEntity>
{
    public Expression<Func<VendorEntity, bool>> Criteria { get; }

    public List<SpecificationOrderCriteria<VendorEntity>> OrderCriteria { get; } = [];

    public PotentialVendorsSpecification(string serviceCategory, string county, string state, bool? isCompliant = null)
    {
        Criteria = vendor => vendor.ServiceCategories.Any(sc =>
                        sc.ServiceCategory.Name == serviceCategory &&
                        (isCompliant.HasValue ? sc.IsCompliant == isCompliant.Value : true)) &&
                        vendor.Location.County == county &&
                        vendor.Location.State == state;

        OrderCriteria.Add(
            new(vendor => vendor.ServiceCategories.First(x => x.ServiceCategory.Name == serviceCategory).IsCompliant,
            isAscending: false)
        );
    }
}
