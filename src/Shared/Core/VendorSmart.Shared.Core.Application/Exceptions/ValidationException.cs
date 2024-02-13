using VendorSmart.Shared.Core.Util.Exceptions;

namespace VendorSmart.Shared.Core.Application.Exceptions;
public class ValidationException : AppException
{
    public ValidationException()
        : base(type: "FieldValidation", message: "Error validating fields")
    {
    }
}
