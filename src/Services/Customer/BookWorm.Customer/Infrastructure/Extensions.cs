using Bogus;
using BookWorm.Constants;
using BookWorm.Customer.Domain;
using BookWorm.SharedKernel.EF;
using BookWorm.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookWorm.Customer.Infrastructure;

public static class Extensions
{
    public static void AddPersistence(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMigration<CustomerDbContext>();

        builder.AddNpgsqlDbContext<CustomerDbContext>(
            ServiceName.Database.Customer,
            configureDbContextOptions: dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder
                    .UseNpgsql(optionsBuilder =>
                    {
                        optionsBuilder.MigrationsAssembly(
                            typeof(CustomerDbContext).Assembly.FullName
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
                                    .Set<Consumer>()
                                    .AnyAsync(cancellationToken: cancellationToken)
                            )
                            {
                                var customerFaker = new Faker<Consumer>()
                                    .UseSeed(Seeder.DefaultSeed)
                                    .RuleFor(x => x.FirstName, f => f.Person.FirstName)
                                    .RuleFor(x => x.LastName, f => f.Person.LastName)
                                    .RuleFor(x => x.Email, f => f.Person.Email)
                                    .RuleFor(x => x.PhoneNumber, f => f.Person.Phone)
                                    .RuleFor(
                                        x => x.DateOfBirth,
                                        f => DateOnly.FromDateTime(f.Person.DateOfBirth)
                                    )
                                    .RuleFor(
                                        x => x.Address,
                                        f => new Address(
                                            f.Address.StreetAddress(),
                                            f.Address.City(),
                                            f.Address.State(),
                                            f.Address.Country(),
                                            f.Address.ZipCode()
                                        )
                                    );

                                var customersToSeed = customerFaker.Generate(Seeder.DefaultAmount);
                                context.Set<Consumer>().AddRange(customersToSeed);
                                await context.SaveChangesAsync(cancellationToken);
                            }
                        }
                    );
            }
        );

        builder.Services.AddScoped<IDatabaseFacade>(p => p.GetRequiredService<CustomerDbContext>());
        builder.Services.AddScoped(typeof(IReadRepository<>), typeof(CustomerRepository<>));
        builder.Services.AddScoped(typeof(IRepository<>), typeof(CustomerRepository<>));
    }
}
