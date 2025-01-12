using Ardalis.GuardClauses;
using BookWorm.SharedKernel.Core.Model;

namespace BookWorm.Catalog.Domain.BookAggregate;

public sealed class BookAuthor : Entity
{
    public BookAuthor()
    {
        // EF Core
    }

    public Guid AuthorId { get; private set; }
    public Author Author { get; private set; } = default!;
    public Book Book { get; private set; } = default!;

    public BookAuthor(Guid authorId)
        : this()
    {
        AuthorId = Guard.Against.Default(authorId);
    }
}
