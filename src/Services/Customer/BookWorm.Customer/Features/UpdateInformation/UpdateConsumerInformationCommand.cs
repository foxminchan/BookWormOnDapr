using Ardalis.Result;
using BookWorm.Customer.Domain;
using BookWorm.Customer.Domain.Specifications;
using BookWorm.SharedKernel.Command;
using BookWorm.SharedKernel.Repositories;

namespace BookWorm.Customer.Features.Update;

internal sealed record UpdateConsumerInformationCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    DateOnly? DateOfBirth
) : ICommand;

internal sealed class UpdateConsumerHandler(IRepository<Consumer> repository)
    : ICommandHandler<UpdateConsumerInformationCommand>
{
    public async Task<Result> Handle(
        UpdateConsumerInformationCommand request,
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

        consumer.UpdateInformation(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.DateOfBirth
        );

        await repository.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
