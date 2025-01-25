using Bogus;
using BookWorm.Inventory.Domain;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookWorm.Inventory.Infrastructure;

public static class Extensions
{
    public static void AddPersistence(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMigration<InventoryDbContext>();

        builder.AddNpgsqlDbContext<InventoryDbContext>(
            ServiceName.Database.Inventory,
            configureDbContextOptions: dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder
                    .UseNpgsql(optionsBuilder =>
                    {
                        optionsBuilder.MigrationsAssembly(
                            typeof(InventoryDbContext).Assembly.FullName
                        );
                        optionsBuilder.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    })
                    .ConfigureWarnings(warnings =>
                        warnings.Ignore(RelationalEventId.PendingModelChangesWarning)
                    )
                    .UseAsyncSeeding(
                        async (context, _, cancellationToken) =>
                        {
                            if (builder.Environment.IsProduction())
                            {
                                return;
                            }

                            if (
                                !await context
                                    .Set<Warehouse>()
                                    .AnyAsync(cancellationToken: cancellationToken)
                            )
                            {
                                var warehouseFaker = new Faker<Warehouse>()
                                    .UseSeed(Seeder.DefaultSeed)
                                    .RuleFor(x => x.Description, f => f.Commerce.ProductAdjective())
                                    .RuleFor(x => x.Status, f => f.PickRandom<WarehouseStatus>())
                                    .RuleFor(x => x.Location, f => f.Address.FullAddress())
                                    .RuleFor(x => x.Website, f => f.Internet.Url());

                                var warehousesToSeed = warehouseFaker.Generate(
                                    Seeder.DefaultAmount
                                );
                                context.Set<Warehouse>().AddRange(warehousesToSeed);
                                await context.SaveChangesAsync(cancellationToken);
                            }
                        }
                    );
            }
        );

        builder.Services.AddScoped<IDatabaseFacade>(p =>
            p.GetRequiredService<InventoryDbContext>()
        );
        builder.Services.AddScoped(typeof(IReadRepository<>), typeof(InventoryRepository<>));
        builder.Services.AddScoped(typeof(IRepository<>), typeof(InventoryRepository<>));
    }
}
