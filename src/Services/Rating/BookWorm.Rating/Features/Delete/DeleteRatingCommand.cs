using BookWorm.Rating.Domain;

namespace BookWorm.Rating.Features.Delete;

internal sealed record DeleteRatingCommand(Guid Id) : ICommand;

internal sealed class DeleteRatingHandler(IRepository<Review> repository)
    : ICommandHandler<DeleteRatingCommand>
{
    public async Task<Result> Handle(
        DeleteRatingCommand request,
        CancellationToken cancellationToken
    )
    {
        var review = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (review is null)
        {
            return Result.NotFound();
        }

        review.Delete();
        await repository.DeleteAsync(review, cancellationToken);

        return Result.NoContent();
    }
}
