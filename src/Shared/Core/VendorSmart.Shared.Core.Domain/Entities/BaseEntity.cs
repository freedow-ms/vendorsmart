namespace VendorSmart.Shared.Core.Domain;

public class BaseEntity
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; init; }

    public BaseEntity(Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }
}