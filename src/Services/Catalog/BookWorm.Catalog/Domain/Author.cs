using Ardalis.GuardClauses;
using BookWorm.Catalog.Domain.BookAggregate;
using BookWorm.SharedKernel.Core.Model;

namespace BookWorm.Catalog.Domain;

public sealed class Author : Entity, IAggregateRoot
{
    public Author()
    {
        // EF Core
    }

    public string? Name { get; private set; }

    private readonly List<BookAuthor> _bookAuthors = [];
    public IReadOnlyCollection<BookAuthor> BookAuthors => _bookAuthors.AsReadOnly();

    public Author(string name)
        : this()
    {
        Name = Guard.Against.NullOrEmpty(name);
    }
}
