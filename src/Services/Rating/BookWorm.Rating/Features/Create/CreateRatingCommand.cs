using System.Text.Json.Serialization;
using BookWorm.Rating.Domain;

namespace BookWorm.Rating.Features.Create;

internal sealed record CreateRatingCommand(
    [property: JsonIgnore] Guid CustomerId,
    int Rating,
    string? Comment,
    Guid BookId
) : ICommand<Result<Guid>>;

internal sealed class CreateRatingHandler(IRepository<Review> repository)
    : ICommandHandler<CreateRatingCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateRatingCommand request,
        CancellationToken cancellationToken
    )
    {
        var review = new Review(
            request.Rating,
            request.Comment,
            request.BookId,
            request.CustomerId
        );

        var result = await repository.AddAsync(review, cancellationToken);

        return result.Id;
    }
}
