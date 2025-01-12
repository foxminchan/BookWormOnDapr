using Ardalis.GuardClauses;
using BookWorm.SharedKernel.Core.Model;

namespace BookWorm.Catalog.Domain.BookAggregate;

public sealed class BookCategory : Entity
{
    public BookCategory()
    {
        // EF Core
    }

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = default!;
    public Book Book { get; private set; } = default!;

    public BookCategory(Guid categoryId)
        : this()
    {
        CategoryId = Guard.Against.Default(categoryId);
    }
}
