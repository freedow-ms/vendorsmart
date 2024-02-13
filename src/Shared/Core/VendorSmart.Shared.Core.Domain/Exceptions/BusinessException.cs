using VendorSmart.Shared.Core.Util.Exceptions;

namespace VendorSmart.Shared.Core.Domain.Exceptions;

public class BusinessException(string type, string message) : AppException(type: type, message: message)
{
}
