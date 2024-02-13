namespace VendorSmart.Shared.Core.Application;

public record ProblemDetails(string? Type = default, int? Status = default, Dictionary<string, string[]>? Errors = default, string? Title = default, string? Details = default);