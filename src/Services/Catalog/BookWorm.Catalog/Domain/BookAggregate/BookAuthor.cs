namespace BookWorm.Catalog.Domain.BookAggregate;

public sealed class BookAuthor() : Entity
{
    public Guid AuthorId { get; private set; }
    public Author Author { get; private set; } = null!;
    public Book Book { get; private set; } = null!;

    public BookAuthor(Guid authorId)
        : this()
    {
        AuthorId = Guard.Against.Default(authorId);
    }
}
