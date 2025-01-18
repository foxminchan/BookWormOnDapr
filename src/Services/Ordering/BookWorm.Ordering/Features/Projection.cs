using System.ComponentModel;
using BookWorm.Ordering.Domain;
using BookWorm.Ordering.Domain.Events;
using BookWorm.SharedKernel.Endpoints;
using Marten;
using Marten.AspNetCore;
using Marten.Events.Projections;

namespace BookWorm.Ordering.Features;

public sealed class OrderSummaryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/orders/{id:guid}/summary",
                (
                    HttpContext context,
                    IQuerySession querySession,
                    [Description("The order id")] Guid id
                ) => querySession.Json.WriteById<OrderSummaryQuery>(id, context)
            )
            .Produces<OrderSummaryQuery>()
            .WithOpenApi()
            .WithTags(nameof(Ordering))
            .MapToApiVersion(new(1, 0));
    }
}

public sealed class OrderSummaryQuery
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
}

public sealed class Projection : MultiStreamProjection<OrderSummaryQuery, Guid>
{
    public Projection()
    {
        Identity<OrderPlacedEvent>(e => e.Id);
        Identity<OrderPaidEvent>(e => e.Id);
        Identity<OrderCancelledEvent>(e => e.Id);
        Identity<OrderCompletedEvent>(e => e.Id);
    }

    public OrderSummaryQuery Apply(OrderSummaryQuery query, OrderPlacedEvent @event)
    {
        query.Status = OrderStatus.New;
        query.TotalPrice = @event.TotalPrice;
        return query;
    }

    public OrderSummaryQuery Apply(OrderSummaryQuery query, OrderPaidEvent @event)
    {
        query.Status = OrderStatus.Paid;
        query.TotalPrice = @event.TotalPrice;
        return query;
    }

    public OrderSummaryQuery Apply(OrderSummaryQuery query, OrderCancelledEvent @event)
    {
        query.Status = OrderStatus.Cancelled;
        query.TotalPrice = @event.TotalPrice;
        return query;
    }

    public OrderSummaryQuery Apply(OrderSummaryQuery query, OrderCompletedEvent @event)
    {
        query.Status = OrderStatus.Completed;
        query.TotalPrice = @event.TotalPrice;
        return query;
    }
}
