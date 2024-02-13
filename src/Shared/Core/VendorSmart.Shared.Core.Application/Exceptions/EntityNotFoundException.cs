using VendorSmart.Shared.Core.Util.Exceptions;

namespace VendorSmart.Shared.Core.Application.Exceptions;

public class EntityNotFoundException : AppException
{
    public EntityNotFoundException()
        : base(type: "Invalid entity", message: "Entity not found")
    {
    }
}
