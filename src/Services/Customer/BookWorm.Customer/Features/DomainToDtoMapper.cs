using BookWorm.Customer.Domain;

namespace BookWorm.Customer.Features;

public static class DomainToDtoMapper
{
    public static ConsumerDto ToConsumerDto(this Consumer consumer)
    {
        return new ConsumerDto(
            consumer.Id,
            consumer.FirstName,
            consumer.LastName,
            consumer.Email,
            consumer.PhoneNumber,
            consumer.DateOfBirth,
            consumer.Address?.ToAddressDto()
        );
    }

    public static AddressDto ToAddressDto(this Address address)
    {
        return new AddressDto(
            address.Street,
            address.City,
            address.State,
            address.Country,
            address.ZipCode
        );
    }

    public static IReadOnlyList<ConsumerDto> ToConsumerDtos(this List<Consumer>? consumers)
    {
        return consumers?.Select(ToConsumerDto).ToList() ?? [];
    }
}
