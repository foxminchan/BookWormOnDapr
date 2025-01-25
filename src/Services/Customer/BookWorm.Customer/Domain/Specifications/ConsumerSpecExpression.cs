namespace BookWorm.Customer.Domain.Specifications;

public static class ConsumerSpecExpression
{
    public static ISpecificationBuilder<Consumer> ApplyOrdering(
        this ISpecificationBuilder<Consumer> builder,
        string? orderBy,
        bool isDescending
    )
    {
        return orderBy switch
        {
            nameof(Consumer.Id) => isDescending
                ? builder.OrderByDescending(x => x.Id)
                : builder.OrderBy(x => x.Id),
            nameof(Consumer.FirstName) => isDescending
                ? builder.OrderByDescending(x => x.FirstName)
                : builder.OrderBy(x => x.FirstName),
            nameof(Consumer.LastName) => isDescending
                ? builder.OrderByDescending(x => x.LastName)
                : builder.OrderBy(x => x.LastName),
            nameof(Consumer.Email) => isDescending
                ? builder.OrderByDescending(x => x.Email)
                : builder.OrderBy(x => x.Email),
            nameof(Consumer.PhoneNumber) => isDescending
                ? builder.OrderByDescending(x => x.PhoneNumber)
                : builder.OrderBy(x => x.PhoneNumber),
            _ => isDescending ? builder.OrderByDescending(x => x.Id) : builder.OrderBy(x => x.Id),
        };
    }

    public static ISpecificationBuilder<Consumer> ApplyPaging(
        this ISpecificationBuilder<Consumer> builder,
        int pageIndex,
        int pageSize
    )
    {
        return builder.Skip((pageIndex - 1) * pageSize).Take(pageSize);
    }
}
