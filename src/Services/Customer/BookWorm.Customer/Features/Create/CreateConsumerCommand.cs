using BookWorm.Customer.Domain;

namespace BookWorm.Customer.Features.Create;

internal sealed record CreateConsumerCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    DateOnly? DateOfBirth,
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode
) : ICommand<Result<Guid>>;

internal sealed class CreateConsumerHandler(IRepository<Consumer> repository)
    : ICommandHandler<CreateConsumerCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateConsumerCommand request,
        CancellationToken cancellationToken
    )
    {
        var consumer = new Consumer(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            request.DateOfBirth,
            new(request.Street, request.City, request.State, request.Country, request.ZipCode)
        );

        var result = await repository.AddAsync(consumer, cancellationToken);

        return result.Id;
    }
}
