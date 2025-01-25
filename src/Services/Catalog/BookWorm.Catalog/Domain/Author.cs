using BookWorm.Catalog.Domain.BookAggregate;

namespace BookWorm.Catalog.Domain;

public sealed class Author() : Entity, IAggregateRoot
{
    public string? Name { get; private set; }

    private readonly List<BookAuthor> _bookAuthors = [];
    public IReadOnlyCollection<BookAuthor> BookAuthors => _bookAuthors.AsReadOnly();

    public Author(string name)
        : this()
    {
        Name = Guard.Against.NullOrEmpty(name);
    }
}
