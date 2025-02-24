// <auto-generated/>
#pragma warning disable
using BookWorm.Ordering.Features;
using Marten;
using Marten.Events.Aggregation;
using Marten.Internal.Storage;
using System;
using System.Linq;

namespace Marten.Generated.EventStore
{
    // START: ProjectionLiveAggregation1847447370
    public class ProjectionLiveAggregation1847447370 : Marten.Events.Aggregation.SyncLiveAggregatorBase<BookWorm.Ordering.Features.OrderSummaryQuery>
    {
        private readonly BookWorm.Ordering.Features.Projection _projection;

        public ProjectionLiveAggregation1847447370(BookWorm.Ordering.Features.Projection projection)
        {
            _projection = projection;
        }



        public override BookWorm.Ordering.Features.OrderSummaryQuery Build(System.Collections.Generic.IReadOnlyList<Marten.Events.IEvent> events, Marten.IQuerySession session, BookWorm.Ordering.Features.OrderSummaryQuery snapshot)
        {
            if (!events.Any()) return snapshot;
            var usedEventOnCreate = snapshot is null;
            snapshot ??= Create(events[0], session);;
            if (snapshot is null)
            {
                usedEventOnCreate = false;
                snapshot = CreateDefault(events[0]);
            }

            foreach (var @event in events.Skip(usedEventOnCreate ? 1 : 0))
            {
                snapshot = Apply(@event, snapshot, session);
            }

            return snapshot;
        }


        public BookWorm.Ordering.Features.OrderSummaryQuery Create(Marten.Events.IEvent @event, Marten.IQuerySession session)
        {
            return null;
        }


        public BookWorm.Ordering.Features.OrderSummaryQuery Apply(Marten.Events.IEvent @event, BookWorm.Ordering.Features.OrderSummaryQuery aggregate, Marten.IQuerySession session)
        {
            switch (@event)
            {
                case Marten.Events.IEvent<BookWorm.Ordering.Domain.Events.OrderCancelledEvent> event_OrderCancelledEvent3:
                    aggregate = _projection.Apply(aggregate, event_OrderCancelledEvent3.Data);
                    break;
                case Marten.Events.IEvent<BookWorm.Ordering.Domain.Events.OrderCompletedEvent> event_OrderCompletedEvent4:
                    aggregate = _projection.Apply(aggregate, event_OrderCompletedEvent4.Data);
                    break;
                case Marten.Events.IEvent<BookWorm.Ordering.Domain.Events.OrderPaidEvent> event_OrderPaidEvent2:
                    aggregate = _projection.Apply(aggregate, event_OrderPaidEvent2.Data);
                    break;
                case Marten.Events.IEvent<BookWorm.Ordering.Domain.Events.OrderPlacedEvent> event_OrderPlacedEvent1:
                    aggregate = _projection.Apply(aggregate, event_OrderPlacedEvent1.Data);
                    break;
            }

            return aggregate;
        }

    }

    // END: ProjectionLiveAggregation1847447370
    
    
    // START: ProjectionInlineHandler1847447370
    public class ProjectionInlineHandler1847447370 : Marten.Events.Aggregation.CrossStreamAggregationRuntime<BookWorm.Ordering.Features.OrderSummaryQuery, System.Guid>
    {
        private readonly Marten.IDocumentStore _store;
        private readonly Marten.Events.Aggregation.IAggregateProjection _projection1;
        private readonly Marten.Events.Aggregation.IEventSlicer<BookWorm.Ordering.Features.OrderSummaryQuery, System.Guid> _slicer;
        private readonly Marten.Internal.Storage.IDocumentStorage<BookWorm.Ordering.Features.OrderSummaryQuery, System.Guid> _storage;
        private readonly BookWorm.Ordering.Features.Projection _projection2;

        public ProjectionInlineHandler1847447370(Marten.IDocumentStore store, Marten.Events.Aggregation.IAggregateProjection __projection1, Marten.Events.Aggregation.IEventSlicer<BookWorm.Ordering.Features.OrderSummaryQuery, System.Guid> slicer, Marten.Internal.Storage.IDocumentStorage<BookWorm.Ordering.Features.OrderSummaryQuery, System.Guid> storage, BookWorm.Ordering.Features.Projection __projection2) : base(store, __projection1, slicer, storage)
        {
            _store = store;
            _projection1 = __projection1;
            _slicer = slicer;
            _storage = storage;
            _projection2 = __projection2;
        }



        public override async System.Threading.Tasks.ValueTask<BookWorm.Ordering.Features.OrderSummaryQuery> ApplyEvent(Marten.IQuerySession session, Marten.Events.Projections.EventSlice<BookWorm.Ordering.Features.OrderSummaryQuery, System.Guid> slice, Marten.Events.IEvent evt, BookWorm.Ordering.Features.OrderSummaryQuery aggregate, System.Threading.CancellationToken cancellationToken)
        {
            switch (evt)
            {
                case Marten.Events.IEvent<BookWorm.Ordering.Domain.Events.OrderCancelledEvent> event_OrderCancelledEvent7:
                    aggregate ??= CreateDefault(evt);
                    aggregate = _projection2.Apply(aggregate, event_OrderCancelledEvent7.Data);
                    return aggregate;
                case Marten.Events.IEvent<BookWorm.Ordering.Domain.Events.OrderCompletedEvent> event_OrderCompletedEvent8:
                    aggregate ??= CreateDefault(evt);
                    aggregate = _projection2.Apply(aggregate, event_OrderCompletedEvent8.Data);
                    return aggregate;
                case Marten.Events.IEvent<BookWorm.Ordering.Domain.Events.OrderPaidEvent> event_OrderPaidEvent6:
                    aggregate ??= CreateDefault(evt);
                    aggregate = _projection2.Apply(aggregate, event_OrderPaidEvent6.Data);
                    return aggregate;
                case Marten.Events.IEvent<BookWorm.Ordering.Domain.Events.OrderPlacedEvent> event_OrderPlacedEvent5:
                    aggregate ??= CreateDefault(evt);
                    aggregate = _projection2.Apply(aggregate, event_OrderPlacedEvent5.Data);
                    return aggregate;
            }

            return aggregate;
        }


        public BookWorm.Ordering.Features.OrderSummaryQuery Create(Marten.Events.IEvent @event, Marten.IQuerySession session)
        {
            return null;
        }

    }

    // END: ProjectionInlineHandler1847447370
    
    
}

