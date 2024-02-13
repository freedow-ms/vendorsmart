namespace VendorSmart.Shared.Core.Domain.Contracts;

public interface IDbContext
{
    Task SaveChangesAsync();
}
