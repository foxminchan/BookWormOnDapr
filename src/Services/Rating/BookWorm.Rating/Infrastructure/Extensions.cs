namespace BookWorm.Rating.Infrastructure;

public static class Extensions
{
    public static void AddPersistence(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMigration<RatingDbContext>();

        builder.AddNpgsqlDbContext<RatingDbContext>(
            ServiceName.Database.Rating,
            configureDbContextOptions: dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder
                    .UseNpgsql(optionsBuilder =>
                    {
                        optionsBuilder.MigrationsAssembly(
                            typeof(RatingDbContext).Assembly.FullName
                        );
                        optionsBuilder.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    })
                    .ConfigureWarnings(warnings =>
                        warnings.Ignore(RelationalEventId.PendingModelChangesWarning)
                    );
            }
        );

        builder.Services.AddScoped<IDatabaseFacade>(p => p.GetRequiredService<RatingDbContext>());
        builder.Services.AddScoped(typeof(IReadRepository<>), typeof(RatingRepository<>));
        builder.Services.AddScoped(typeof(IRepository<>), typeof(RatingRepository<>));
    }
}
