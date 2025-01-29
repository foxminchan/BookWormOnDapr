namespace BookWorm.Customer.Domain.Specifications;

public sealed class ConsumerFilterSpec : Specification<Consumer>
{
    public ConsumerFilterSpec(
        string? email,
        string? phoneNumber,
        string? name,
        string? street,
        string? city,
        string? state,
        string? country,
        string? zipCode
    )
    {
        Query.Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(email))
        {
            Query.Where(x => x.Email == email);
        }

        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            Query.Where(x => x.PhoneNumber == phoneNumber);
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            Query.Where(x => x.FirstName!.Contains(name) || x.LastName!.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(street))
        {
            Query.Where(x => x.Address!.Street!.Contains(street));
        }

        if (!string.IsNullOrWhiteSpace(city))
        {
            Query.Where(x => x.Address!.City!.Contains(city));
        }

        if (!string.IsNullOrWhiteSpace(state))
        {
            Query.Where(x => x.Address!.State!.Contains(state));
        }

        if (!string.IsNullOrWhiteSpace(country))
        {
            Query.Where(x => x.Address!.Country!.Contains(country));
        }

        if (!string.IsNullOrWhiteSpace(zipCode))
        {
            Query.Where(x => x.Address!.ZipCode!.Contains(zipCode));
        }
    }

    public ConsumerFilterSpec(
        int pageIndex,
        int pageSize,
        string? orderBy,
        bool isDescending,
        string? email,
        string? phoneNumber,
        string? name,
        string? street,
        string? city,
        string? state,
        string? country,
        string? zipCode
    )
        : this(email, phoneNumber, name, street, city, state, country, zipCode)
    {
        Query.ApplyOrdering(orderBy, isDescending).ApplyPaging(pageIndex, pageSize);
    }

    public ConsumerFilterSpec(Guid id)
    {
        Query.Where(x => x.Id == id && !x.IsDeleted);
    }

    public ConsumerFilterSpec(Guid[] ids)
    {
        Query.Where(x => ids.Contains(x.Id) && !x.IsDeleted);
    }
}
