﻿using BookWorm.Basket.Domain;
using Dapr.Client;

namespace BookWorm.Basket.Repositories;

public sealed class DaprBasketRepository(DaprClient daprClient) : IBasketRepository
{
    private static readonly string _prefix = nameof(Basket).ToLowerInvariant();
    private const string StateStoreName = ServiceName.Component.Store;

    public async Task<Guid> CreateAsync(Card card, CancellationToken cancellationToken)
    {
        await daprClient.SaveStateAsync(
            StateStoreName,
            $"{_prefix}:{card.CustomerId}",
            card,
            cancellationToken: cancellationToken
        );

        return card.CustomerId;
    }

    public async Task DeleteAsync(Guid? customerId, CancellationToken cancellationToken)
    {
        await daprClient.DeleteStateAsync(
            StateStoreName,
            $"{_prefix}-{customerId}",
            cancellationToken: cancellationToken
        );
    }

    public async Task<Card?> GetAsync(Guid? customerId, CancellationToken cancellationToken)
    {
        var state = await daprClient.GetStateAsync<Card>(
            StateStoreName,
            $"{_prefix}:{customerId}",
            cancellationToken: cancellationToken
        );

        return state;
    }

    public async Task UpdateAsync(Card card, CancellationToken cancellationToken)
    {
        var state = await daprClient.GetStateEntryAsync<Card>(
            StateStoreName,
            $"{_prefix}:{card.CustomerId}",
            cancellationToken: cancellationToken
        );

        state.Value = card;

        await state.SaveAsync(cancellationToken: cancellationToken);
    }
}
