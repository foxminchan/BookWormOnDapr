using Ardalis.Result;
using BookWorm.Customer.Domain;
using BookWorm.Customer.Domain.Specifications;
using BookWorm.SharedKernel.Query;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Customer.Features.Get;

internal sealed record GetConsumerQuery(Guid Id) : IQuery<Result<ConsumerDto>>;

internal sealed class GetConsumerHandler(IReadRepository<Consumer> repository)
    : IQueryHandler<GetConsumerQuery, Result<ConsumerDto>>
{
    public async Task<Result<ConsumerDto>> Handle(
        GetConsumerQuery request,
        CancellationToken cancellationToken
    )
    {
        var consumer = await repository.FirstOrDefaultAsync(
            new ConsumerFilterSpec(request.Id),
            cancellationToken
        );

        if (consumer is null)
        {
            return Result.NotFound();
        }

        return consumer.ToConsumerDto();
    }
}
