using BookWorm.Rating.Domain;
using BookWorm.Rating.Domain.Specifications;

namespace BookWorm.Rating.Features.List;

internal sealed record ListRatingsQuery(
    [property: Description("The book id")] Guid BookId,
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(Pagination.DefaultPageIndex)]
        int PageIndex = Pagination.DefaultPageIndex,
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(Pagination.DefaultPageSize)]
        int PageSize = Pagination.DefaultPageSize,
    [property: Description("Property to order results by")]
    [property: DefaultValue(nameof(Review.Rating))]
        string? OrderBy = nameof(Review.Rating),
    [property: Description("Whether to order results in descending order")]
    [property: DefaultValue(false)]
        bool IsDescending = false
) : IQuery<PagedResult<IReadOnlyList<ReviewDto>>>;

internal sealed class ListRatingsHandler(IReadRepository<Review> repository, DaprClient daprClient)
    : IQueryHandler<ListRatingsQuery, PagedResult<IReadOnlyList<ReviewDto>>>
{
    public async Task<PagedResult<IReadOnlyList<ReviewDto>>> Handle(
        ListRatingsQuery request,
        CancellationToken cancellationToken
    )
    {
        var reviews = repository.ListAsync(
            new ReviewFilterSpec(
                request.BookId,
                request.PageIndex,
                request.PageSize,
                request.OrderBy,
                request.IsDescending
            ),
            cancellationToken
        );

        var totalRecords = repository.CountAsync(
            new ReviewFilterSpec(request.BookId),
            cancellationToken
        );

        await Task.WhenAll(reviews, totalRecords);

        var customerIds = reviews.Result.Select(x => x.CustomerId).Distinct().ToArray();

        Dictionary<Guid, string> customers = [];
        if (customerIds is { Length: > 0 })
        {
            customers = await daprClient.InvokeMethodAsync<Dictionary<Guid, string>>(
                HttpMethod.Get,
                ServiceName.App.Customer,
                "/api/v1/customers/by-ids?ids=" + string.Join(",", customerIds),
                cancellationToken
            );
        }

        var reviewsDtos = reviews.Result.ToReviewDtos(customers);

        var totalPages = (int)Math.Ceiling(totalRecords.Result / (double)request.PageSize);

        PagedInfo pagedInfo = new(
            request.PageIndex,
            request.PageSize,
            totalRecords.Result,
            totalPages
        );

        return new(pagedInfo, reviewsDtos);
    }
}
