using Ardalis.GuardClauses;
using BookWorm.SharedKernel.Core.Model;
using BookWorm.SharedKernel.Models;

namespace BookWorm.Customer.Domain;

public sealed class Consumer : AuditableEntity, IAggregateRoot, ISoftDelete
{
    public Consumer()
    {
        // EF Core
    }

    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public bool IsDeleted { get; set; }
    public string? AccountId { get; private set; }
    public Address? Address { get; private set; }

    public Consumer(
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        DateOnly? dateOfBirth,
        Address address
    )
        : this()
    {
        FirstName = Guard.Against.NullOrEmpty(firstName);
        LastName = Guard.Against.NullOrEmpty(lastName);
        Email = Guard.Against.NullOrEmpty(email);
        PhoneNumber = Guard.Against.NullOrEmpty(phoneNumber);
        DateOfBirth = dateOfBirth;
        Address = Guard.Against.Null(address);
    }

    public void UpdateInformation(
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        DateOnly? dateOfBirth
    )
    {
        FirstName = Guard.Against.NullOrEmpty(firstName);
        LastName = Guard.Against.NullOrEmpty(lastName);
        Email = Guard.Against.NullOrEmpty(email);
        PhoneNumber = Guard.Against.NullOrEmpty(phoneNumber);
        DateOfBirth = dateOfBirth;
    }

    public void UpdateAddress(Address address)
    {
        Address = Guard.Against.Null(address);
    }

    public void SetAccountId(string accountId)
    {
        AccountId = Guard.Against.NullOrEmpty(accountId);
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}
