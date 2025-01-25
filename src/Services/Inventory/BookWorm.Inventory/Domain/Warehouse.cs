namespace BookWorm.Inventory.Domain;

public sealed class Warehouse() : AuditableEntity<long>, IAggregateRoot, ISoftDelete
{
    public string? Location { get; private set; }
    public WarehouseStatus Status { get; private set; }
    public string? Description { get; private set; }
    public string? Website { get; private set; }
    public bool IsDeleted { get; set; }

    private readonly List<Stock> _stocks = [];
    public IReadOnlyCollection<Stock> Stocks => _stocks.AsReadOnly();

    public Warehouse(
        string location,
        WarehouseStatus status,
        string? description,
        string? website,
        List<Stock> stocks
    )
        : this()
    {
        Location = Guard.Against.NullOrEmpty(location);
        Status = Guard.Against.EnumOutOfRange(status);
        Description = description;
        Website = website;
        _stocks = stocks;
    }

    public void Delete()
    {
        IsDeleted = true;
    }

    /// <summary>
    /// Add stock to the warehouse
    /// </summary>
    /// <param name="stocks">The list of stocks to add</param>
    public void AddStocks(List<Stock> stocks)
    {
        var existingStocksMap = _stocks
            .Where(s => stocks.Any(stock => stock.ProductId == s.ProductId))
            .ToDictionary(s => s.ProductId);

        foreach (var stock in stocks)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(stock.Quantity, 1, nameof(stock.Quantity));

            if (existingStocksMap.TryGetValue(stock.ProductId, out var existingStock))
            {
                existingStock.IncreaseQuantity(stock.Quantity);
            }
            else
            {
                _stocks.Add(stock);
            }
        }
    }

    /// <summary>
    /// Remove stock from the warehouse
    /// </summary>
    /// <param name="productId">The product id</param>
    /// <param name="quantity">The quantity to remove</param>
    public void RemoveStock(Guid productId, int quantity)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(quantity, 1, nameof(quantity));

        var existingStock = _stocks.FirstOrDefault(s => s.ProductId == productId);

        if (existingStock is null)
        {
            return;
        }

        existingStock.DecreaseQuantity(quantity);
        if (existingStock.Quantity <= 0)
        {
            _stocks.Remove(existingStock);
        }
    }

    /// <summary>
    /// Get product stock from the warehouse
    /// </summary>
    /// <param name="productId">The product id</param>
    public Stock GetStock(Guid productId)
    {
        return _stocks.First(s => s.ProductId == productId);
    }
}
