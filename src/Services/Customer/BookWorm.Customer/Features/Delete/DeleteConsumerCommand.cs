using BookWorm.Customer.Domain;
using BookWorm.Customer.Domain.Specifications;

namespace BookWorm.Customer.Features.Delete;

internal sealed record DeleteConsumerCommand(Guid Id) : ICommand;

internal sealed class DeleteConsumerHandler(IRepository<Consumer> repository)
    : ICommandHandler<DeleteConsumerCommand>
{
    public async Task<Result> Handle(
        DeleteConsumerCommand request,
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

        consumer.Delete();

        await repository.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
