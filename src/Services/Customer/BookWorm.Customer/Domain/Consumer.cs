using BookWorm.SharedKernel.Core;

namespace BookWorm.Customer.Domain;

public sealed class Consumer() : AuditableEntity, IAggregateRoot, ISoftDelete
{
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

    /// <summary>
    /// Update consumer information
    /// </summary>
    /// <param name="firstName">First name of the consumer</param>
    /// <param name="lastName">Last name of the consumer</param>
    /// <param name="email">Email address of the consumer</param>
    /// <param name="phoneNumber">Phone number of the consumer</param>
    /// <param name="dateOfBirth">Date of birth of the consumer</param>
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

    /// <summary>
    /// Update consumer address
    /// </summary>
    /// <param name="address">Address of the consumer</param>
    public void UpdateAddress(Address address)
    {
        Address = Guard.Against.Null(address);
    }

    /// <summary>
    /// Assign an account to the consumer
    /// </summary>
    /// <param name="accountId">Account ID</param>
    public void SetAccountId(string accountId)
    {
        AccountId = Guard.Against.NullOrEmpty(accountId);
    }

    /// <summary>
    /// Delete the consumer
    /// </summary>
    public void Delete()
    {
        IsDeleted = true;
    }
}
