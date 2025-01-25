using BookWorm.Customer.Domain;
using BookWorm.Customer.Domain.Specifications;

namespace BookWorm.Customer.Features.UpdateAddress;

internal sealed record UpdateAddressCommand(
    Guid Id,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode
) : ICommand;

internal sealed class UpdateAddressHandler(IRepository<Consumer> repository)
    : ICommandHandler<UpdateAddressCommand>
{
    public async Task<Result> Handle(
        UpdateAddressCommand request,
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

        consumer.UpdateAddress(
            new(request.Street, request.City, request.State, request.Country, request.ZipCode)
        );

        await repository.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
