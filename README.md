# BookWorm: Modern app development

<a href="https://github.com/foxminchan/BookWormOnDapr/blob/main/LICENSE">
	<img alt="build-status" src="https://img.shields.io/github/license/foxminchan/BookWormOnDapr?color=%234275f5&style=flat-square" />
</a>

## Introduction

⭐ This pet project is dedicated to streamlining microservices development using Aspire, Dapr, and the Radius Platform. Its primary objective is to provide a cutting-edge application development experience that prioritizes developer productivity, scalability, and reliability.

💡 The focus of this project is not only on business logic but also on the architecture, design, and development of a modern microservices application. It incorporates various design patterns, principles and concepts commonly used in microservices, including Domain-Driven Design (DDD), pub/sub, Sidecar Pattern, Event Sourcing, and CQRS.

<div>
  <a href="https://codespaces.new/foxminchan/BookWormOnDapr?quickstart=1">
    <img alt="Open in GitHub Codespaces" src="https://github.com/codespaces/badge.svg">
  </a>
</div>

## The Goals of the Project

- [x] Building a cloud-native application with .NET Aspire and Dapr
- [x] Implementing a microservices architecture with DDD, CQRS, and Event Sourcing
- [x] Using the Sidecar Pattern to integrate Dapr with existing applications
- [x] Using Vertical Slice Architecture to organize the codebase
- [x] Using Duende IdentityServer for authentication and authorization
- [x] Using Kafka for pub/sub messaging
- [ ] Using NUKE for build automation
- [ ] Integrating LLM with the application
  - [ ] Leveraging the [Microsoft.Extensions.AI](https://learn.microsoft.com/en-us/dotnet/ai/ai-extensions)
  - [ ] Using Ollama with DeepSeek R2 model
- [ ] Integrating with the Radius Platform for deployment

## Software Architecture

### System Context Diagram

```mermaid
C4Context
  title System Context Diagram for Book Worm

  Person_Ext(customer, "Customer", "Uses the BookWorm app to browse and purchase books")
  Person(admin, "Administrator", "Manages catalog, users, and system configuration")

  Boundary(book_worm_system, "Book Worm System") {
    System(bookworm_app, "BookWorm Application", "Provides web/mobile interface for book browsing, purchasing, and management")

    Boundary(infrastructure, "Infrastructure Services") {
      System(dapr_sidecar, "Dapr Sidecar", "Handles distributed system primitives")
      SystemDb(database, "Database", "Stores product catalog, user data, and transactions")
      SystemQueue(message_bus, "Message Bus", "Facilitates event-driven communication between components")
    }
  }

  System_Ext(email_provider, "Email Service", "Third-party email delivery")

  Rel(customer, bookworm_app, ""Browses/Purchases books", "HTTPS")
  Rel(admin, bookworm_app, "Manages content/config", "HTTPS")
  Rel(bookworm_app, dapr_sidecar, "Uses for state management and messaging", "HTTPS")
  Rel(dapr_sidecar, database, "Persists data using", "SQL/TCP")
  Rel(dapr_sidecar, message_bus, "Publishes/subscribes to events using")
  Rel(dapr_sidecar, email_provider, "Sends transactional emails via", "SMTP/API")
```

### Container Diagram

```mermaid
C4Context
title Container Diagram for Book Worm System

Person_Ext(customer, "Customer", "Uses the BookWorm app to browse and purchase books")
Person(admin, "Administrator", "Manages catalog, users, and system configuration")

Boundary(book_worm_system, "Book Worm System") {
  Container(reverse_proxy, "API Gateway", "C# + YARP", "Single entry point for all API requests")

  Boundary(business_capabilities, "Business Capabilities") {
    Container(catalog_api, "Catalog API", ".NET 9", "Product information management")
    Container(inventory_api, "Inventory API", ".NET 9", "Real-time stock tracking")
    Container(ordering_api, "Order API", ".NET 9 + Marten", "Order processing workflows")
    Container(basket_api, "Basket API", ".NET 9", "Shopping cart operations")
    Container(payment_api, "Payment API", ".NET 9", "Payment processing")
  }

  Boundary(cross_cutting, "Cross-Cutting Services") {
    Container(identity_service, "Identity Service", "Duende IS v7", "Authentication & Authorization")
    Container(customer_api, "Customer API", ".NET 9", "User profile management")
    Container(rating_api, "Rating API", ".NET 9", "Product reviews & ratings")
  }

  Boundary(infrastructure, "Infrastructure Layer") {
    System(dapr, "Dapr Sidecar", "Distributed system building blocks")
    SystemDb(postgres, "PostgreSQL/Redis", "Relational/Key-value store")
    SystemQueue(kafka, "Kafka", "Event streaming backbone")
  }

  Container(notifications, "Notification Service", ".NET 9", "Event-driven notifications")
}

System_Ext(email_provider, "Email Service", "Third-party email provider")

Rel(customer, reverse_proxy, "Browses/Purchases books", "HTTPS")
Rel(admin, reverse_proxy, "Manages content/config", "HTTPS")

Rel(reverse_proxy, catalog_api, "Proxies", "HTTP/HTTPS")
Rel(reverse_proxy, inventory_api, "Proxies", "HTTP/HTTPS")
Rel(reverse_proxy, basket_api, "Proxies", "HTTP/HTTPS")
Rel(reverse_proxy, ordering_api, "Proxies", "HTTP/HTTPS")

Rel(reverse_proxy, identity_service, "Proxies", "HTTP/HTTPS")
Rel(reverse_proxy, customer_api, "Proxies", "HTTP/HTTPS")
Rel(reverse_proxy, rating_api, "Proxies", "HTTP/HTTPS")
Rel(reverse_proxy, payment_api, "Proxies", "HTTP/HTTPS")

Rel(dapr, postgres, "Persists data using", "SQL/TCP")
Rel(dapr, kafka, "Publishes/subscribes to events using")
Rel(dapr, email_provider, "Sends transactional emails via", "SMTP/API")

Rel(catalog_api, dapr, "Uses for pub/sub messaging", "HTTPS")
Rel(identity_service, dapr, "Uses for pub/sub messaging")
Rel(basket_api, dapr, "Uses for pub/sub messaging, state management", "HTTPS")
Rel(ordering_api, dapr, "Uses for pub/sub messaging, state management and workflows", "HTTPS")
Rel(customer_api, dapr, "Uses for pub/sub messaging", "HTTPS")
Rel(rating_api, dapr, "Uses for pub/sub messaging", "HTTPS")
Rel(payment_api, dapr, "Uses for pub/sub messaging", "HTTPS")
Rel(notifications, dapr, "Uses for pub/sub messaging, bindings and actors", "HTTPS")
```

## Getting Started

### Prerequisites

For the development environment, you need to install the following tools:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker](https://www.docker.com/products/docker-desktop)
- [Dapr CLI](https://docs.dapr.io/getting-started/install-dapr-cli/)
- [Gitleaks](https://gitleaks.io/)

With deployment, you need to install the following tools:

- [k3d](https://k3d.io/) & [k9s](https://k9scli.io/)
- [Radius CLI](https://docs.radapp.io/installation/)

### Setting up user secrets

1. Open a terminal
2. Run the following command to set the user secrets for the `BookWorm.AppHost` project

```bash
dotnet user-secrets set "Parameters:SqlUser" "postgres"
dotnet user-secrets set "Parameters:SqlPassword" "yourcomplexpassword"
```

### Running the project

1. Clone the repository

```bash
git clone git@github.com:foxminchan/BookWormOnDapr.git
```

2. Initialize Dapr

```bash
dapr init
```

3. Run the project

```bash
dotnet run --project src/BuildingBlocks/BookWorm.AppHost/BookWorm.AppHost.csproj
```

> [!WARNING]
> Ensure that you have Docker running on your machine.

## Contributing

- Fork the repository
- Create a new branch for your feature
- Make your changes
- Create a pull request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## BookWorm

For a version of this app using Aspire and MassTransit, please visit the [BookWorm](https://github.com/foxminchan/BookWorm) repository.
