using BookWorm.Customer.Domain;
using BookWorm.Customer.Domain.Specifications;

namespace BookWorm.Customer.Features.List;

internal sealed record ListConsumersQuery(
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(Pagination.DefaultPageIndex)]
        int PageIndex = Pagination.DefaultPageIndex,
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(Pagination.DefaultPageSize)]
        int PageSize = Pagination.DefaultPageSize,
    [property: Description("Property to order results by")]
    [property: DefaultValue(nameof(Consumer.Id))]
        string? OrderBy = nameof(Consumer.Id),
    [property: Description("Whether to order results in descending order")]
    [property: DefaultValue(false)]
        bool IsDescending = false,
    [property: Description("Email to filter results by")]
    [property: DefaultValue(null)]
        string? Email = null,
    [property: Description("Phone number to filter results by")]
    [property: DefaultValue(null)]
        string? PhoneNumber = null,
    [property: Description("First name or last name to filter results by")]
    [property: DefaultValue(null)]
        string? Name = null,
    [property: Description("Street to filter results by")]
    [property: DefaultValue(null)]
        string? Street = null,
    [property: Description("City to filter results by")]
    [property: DefaultValue(null)]
        string? City = null,
    [property: Description("State to filter results by")]
    [property: DefaultValue(null)]
        string? State = null,
    [property: Description("Country to filter results by")]
    [property: DefaultValue(null)]
        string? Country = null,
    [property: Description("Zip code to filter results by")]
    [property: DefaultValue(null)]
        string? ZipCode = null
) : IQuery<PagedResult<IReadOnlyList<ConsumerDto>>>;

internal sealed class ListConsumersHandler(IReadRepository<Consumer> repository)
    : IQueryHandler<ListConsumersQuery, PagedResult<IReadOnlyList<ConsumerDto>>>
{
    public async Task<PagedResult<IReadOnlyList<ConsumerDto>>> Handle(
        ListConsumersQuery request,
        CancellationToken cancellationToken
    )
    {
        var consumers = repository.ListAsync(
            new ConsumerFilterSpec(
                request.PageIndex,
                request.PageSize,
                request.OrderBy,
                request.IsDescending,
                request.Email,
                request.PhoneNumber,
                request.Name,
                request.Street,
                request.City,
                request.State,
                request.Country,
                request.ZipCode
            ),
            cancellationToken
        );

        var totalRecords = repository.CountAsync(
            new ConsumerFilterSpec(
                request.Email,
                request.PhoneNumber,
                request.Name,
                request.Street,
                request.City,
                request.State,
                request.Country,
                request.ZipCode
            ),
            cancellationToken
        );

        await Task.WhenAll(consumers, totalRecords);

        var totalPages = (int)Math.Ceiling(totalRecords.Result / (double)request.PageSize);

        PagedInfo pagedInfo = new(
            request.PageIndex,
            request.PageSize,
            totalRecords.Result,
            totalPages
        );

        return new(pagedInfo, consumers.Result.ToConsumerDtos());
    }
}
