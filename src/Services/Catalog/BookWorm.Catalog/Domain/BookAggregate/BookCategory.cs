namespace BookWorm.Catalog.Domain.BookAggregate;

public sealed class BookCategory() : Entity
{
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;
    public Book Book { get; private set; } = null!;

    public BookCategory(Guid categoryId)
        : this()
    {
        CategoryId = Guard.Against.Default(categoryId);
    }
}
