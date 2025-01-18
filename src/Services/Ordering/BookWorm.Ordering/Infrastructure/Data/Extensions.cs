using BookWorm.Constants;
using BookWorm.SharedKernel.EF;
using BookWorm.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookWorm.Ordering.Infrastructure.Data;

public static class Extensions
{
    public static void AddPersistence(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMigration<OrderingDbContext>();

        builder.AddNpgsqlDbContext<OrderingDbContext>(
            ServiceName.Database.Ordering,
            configureDbContextOptions: dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder
                    .UseNpgsql(optionsBuilder =>
                    {
                        optionsBuilder.MigrationsAssembly(
                            typeof(OrderingDbContext).Assembly.FullName
                        );
                        optionsBuilder.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    })
                    .ConfigureWarnings(warnings =>
                        warnings.Ignore(RelationalEventId.PendingModelChangesWarning)
                    );
            }
        );

        builder.Services.AddScoped<IDatabaseFacade>(p => p.GetRequiredService<OrderingDbContext>());
        builder.Services.AddScoped(typeof(IReadRepository<>), typeof(OrderingRepository<>));
        builder.Services.AddScoped(typeof(IRepository<>), typeof(OrderingRepository<>));
    }
}
