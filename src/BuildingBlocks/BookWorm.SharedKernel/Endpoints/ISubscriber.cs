﻿using Dapr;
using Microsoft.AspNetCore.Routing;

namespace BookWorm.SharedKernel.Endpoints;

public interface ISubscriber
{
    TopicOptions TopicOptions { get; set; }
    void MapIntegrationEventEndpoint(IEndpointRouteBuilder app);
}

public interface ISubscriber<in TRequest> : ISubscriber
{
    Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface ISubscriber<in TRequest1, in TRequest2> : ISubscriber
{
    Task HandleAsync(
        TRequest1 request1,
        TRequest2 request2,
        CancellationToken cancellationToken = default
    );
}

public interface ISubscriber<in TRequest1, in TRequest2, in TRequest3> : ISubscriber
{
    Task HandleAsync(
        TRequest1 request1,
        TRequest2 request2,
        TRequest3 request3,
        CancellationToken cancellationToken = default
    );
}
