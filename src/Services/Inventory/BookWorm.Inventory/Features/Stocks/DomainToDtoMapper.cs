using BookWorm.Inventory.Domain;

namespace BookWorm.Inventory.Features.Stocks;

public static class DomainToDtoMapper
{
    public static StockDto ToStockDto(this Stock stock)
    {
        return new StockDto(stock.ProductId, stock.Quantity);
    }

    public static IReadOnlyList<StockDto> ToStockDtos(this IReadOnlyCollection<Stock> stocks)
    {
        return stocks.Select(stock => stock.ToStockDto()).ToList();
    }
}
