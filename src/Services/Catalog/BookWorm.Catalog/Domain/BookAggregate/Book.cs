namespace BookWorm.Catalog.Domain.BookAggregate;

public sealed class Book() : AuditableEntity, IAggregateRoot, ISoftDelete
{
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public string? ImageUrl { get; private set; }
    public decimal Price { get; private set; }
    public double AverageRating { get; private set; }
    public int TotalReviews { get; private set; }
    public bool IsDeleted { get; set; }

    private readonly List<BookAuthor> _bookAuthors = [];
    public IReadOnlyCollection<BookAuthor> BookAuthors => _bookAuthors.AsReadOnly();

    private readonly List<BookCategory> _bookCategories = [];
    public IReadOnlyCollection<BookCategory> BookCategories => _bookCategories.AsReadOnly();

    public Book(
        string name,
        string? description,
        string? imageUrl,
        decimal price,
        Guid[] authorIds,
        Guid[] categoryIds
    )
        : this()
    {
        Name = Guard.Against.NullOrEmpty(name);
        Description = Guard.Against.NullOrEmpty(description);
        ImageUrl = Guard.Against.NullOrEmpty(imageUrl);
        Price = Guard.Against.NegativeOrZero(price);
        _bookAuthors = [.. authorIds.Select(authorId => new BookAuthor(authorId))];
        _bookCategories = [.. categoryIds.Select(categoryId => new BookCategory(categoryId))];
    }

    /// <summary>
    /// Soft delete the book
    /// </summary>
    public void Delete()
    {
        IsDeleted = true;
    }

    /// <summary>
    /// Update the book details
    /// </summary>
    /// <param name="name">Name of the book</param>
    /// <param name="description">A brief description of the book</param>
    /// <param name="imageUrl">File name of the image. The image should be uploaded to the server</param>
    /// <param name="price">The price of the book. The price should be greater than zero</param>
    public void Update(string name, string? description, string? imageUrl, decimal price)
    {
        Name = Guard.Against.NullOrEmpty(name);
        Description = description;
        ImageUrl = imageUrl;
        Price = Guard.Against.NegativeOrZero(price);
    }

    /// <summary>
    /// Add a review to the book, and update the average rating
    /// </summary>
    /// <param name="rating">Rating of the book. The rating should be between 1 and 5</param>
    public void AddReview(int rating)
    {
        AverageRating = (AverageRating * TotalReviews + rating) / (TotalReviews + 1);
        TotalReviews++;
    }

    /// <summary>
    /// Remove a review from the book, and update the average rating
    /// </summary>
    /// <param name="rating">Rating of the book. The rating should be between 1 and 5</param>
    public void RemoveReview(int rating)
    {
        AverageRating = (AverageRating * TotalReviews - rating) / (TotalReviews - 1);
        TotalReviews--;
    }
}
