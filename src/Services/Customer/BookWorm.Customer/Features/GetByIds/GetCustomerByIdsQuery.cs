using BookWorm.Customer.Domain;
using BookWorm.Customer.Domain.Specifications;

namespace BookWorm.Customer.Features.GetByIds;

internal sealed record GetCustomerByIdsQuery(Guid[] Ids) : IQuery<Result<Dictionary<Guid, string>>>;

internal sealed class GetCustomerByIdsHandler(IReadRepository<Consumer> repository)
    : IQueryHandler<GetCustomerByIdsQuery, Result<Dictionary<Guid, string>>>
{
    public async Task<Result<Dictionary<Guid, string>>> Handle(
        GetCustomerByIdsQuery request,
        CancellationToken cancellationToken
    )
    {
        var customers = await repository.ListAsync(
            new ConsumerFilterSpec(request.Ids),
            cancellationToken
        );

        return customers.ToDictionary(c => c.Id, c => $"{c.FirstName} {c.LastName}");
    }
}
