using Ardalis.GuardClauses;
using BookWorm.SharedKernel.Core.Model;

namespace BookWorm.Catalog.Domain.BookAggregate;

public sealed class BookCategory : Entity
{
    public BookCategory()
    {
        Category = default!;
        Book = default!;
    }

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }
    public Book Book { get; private set; }

    public BookCategory(Guid categoryId)
        : this()
    {
        CategoryId = Guard.Against.Default(categoryId);
    }
}
