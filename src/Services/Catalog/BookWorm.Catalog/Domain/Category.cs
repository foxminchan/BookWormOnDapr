using Ardalis.GuardClauses;
using BookWorm.Catalog.Domain.BookAggregate;
using BookWorm.SharedKernel.Core.Model;

namespace BookWorm.Catalog.Domain;

public sealed class Category : Entity, IAggregateRoot
{
    public Category()
    {
        // EF Core
    }

    public string? Name { get; private set; }

    private readonly List<BookCategory> _bookCategories = [];
    public IReadOnlyCollection<BookCategory> BookCategories => _bookCategories.AsReadOnly();

    public Category(string name)
        : this()
    {
        Name = Guard.Against.NullOrEmpty(name);
    }
}
