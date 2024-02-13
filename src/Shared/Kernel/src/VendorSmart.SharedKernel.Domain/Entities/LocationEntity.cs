using VendorSmart.Shared.Core.Domain;

namespace VendorSmart.SharedKernel.Domain.Entities;
public class LocationEntity : BaseEntity
{
    public string County { get; private set; } = default!;
    public string State { get; private set; } = default!;

    public LocationEntity(string county, string state)
    {
        County = county;
        State = state;
    }
}