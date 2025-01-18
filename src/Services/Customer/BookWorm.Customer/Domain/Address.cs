using Ardalis.GuardClauses;
using BookWorm.SharedKernel.Core.Model;

namespace BookWorm.Customer.Domain;

public sealed class Address : ValueObject
{
    public Address() { }

    public string? Street { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? Country { get; private set; }
    public string? ZipCode { get; private set; }

    public Address(string street, string city, string state, string country, string zipCode)
        : this()
    {
        Street = Guard.Against.NullOrEmpty(street);
        City = Guard.Against.NullOrEmpty(city);
        State = Guard.Against.NullOrEmpty(state);
        Country = Guard.Against.NullOrEmpty(country);
        ZipCode = Guard.Against.NullOrEmpty(zipCode);
    }

    public override string ToString()
    {
        return $"{Street}, {City}, {State}, {Country}, {ZipCode}";
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ToString();
    }
}
