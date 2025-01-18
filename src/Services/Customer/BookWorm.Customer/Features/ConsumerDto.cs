namespace BookWorm.Customer.Features;

public sealed record ConsumerDto(
    Guid Id,
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumber,
    DateOnly? DateOfBirth,
    AddressDto? Address
);
