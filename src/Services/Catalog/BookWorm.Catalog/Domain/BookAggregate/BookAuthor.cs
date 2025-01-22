using Ardalis.GuardClauses;
using BookWorm.SharedKernel.Core.Model;

namespace BookWorm.Catalog.Domain.BookAggregate;

public sealed class BookAuthor : Entity
{
    public BookAuthor()
    {
        Author = default!;
        Book = default!;
    }

    public Guid AuthorId { get; private set; }
    public Author Author { get; private set; }
    public Book Book { get; private set; }

    public BookAuthor(Guid authorId)
        : this()
    {
        AuthorId = Guard.Against.Default(authorId);
    }
}
