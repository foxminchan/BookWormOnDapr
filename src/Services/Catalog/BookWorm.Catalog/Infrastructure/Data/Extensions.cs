using Bogus;
using BookWorm.Catalog.Domain;
using BookWorm.Catalog.Domain.BookAggregate;
using BookWorm.Constants;
using BookWorm.SharedKernel.EF;
using BookWorm.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookWorm.Catalog.Infrastructure.Data;

public static class Extensions
{
    public static void AddPersistence(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMigration<CatalogDbContext>();

        builder.AddNpgsqlDbContext<CatalogDbContext>(
            ServiceName.Database.Catalog,
            configureDbContextOptions: dbContextOptionsBuilder =>
            {
                dbContextOptionsBuilder
                    .UseNpgsql(optionsBuilder =>
                    {
                        optionsBuilder.MigrationsAssembly(
                            typeof(CatalogDbContext).Assembly.FullName
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
                                    .Set<Category>()
                                    .AnyAsync(cancellationToken: cancellationToken)
                            )
                            {
                                var categoryFaker = new Faker<Category>()
                                    .UseSeed(Seeder.DefaultSeed)
                                    .RuleFor(x => x.Name, f => f.Person.FullName);

                                var categoriesToSeed = categoryFaker.Generate(Seeder.DefaultAmount);

                                context.Set<Category>().AddRange(categoriesToSeed);
                                await context.SaveChangesAsync(cancellationToken);
                            }

                            if (
                                !await context
                                    .Set<Author>()
                                    .AnyAsync(cancellationToken: cancellationToken)
                            )
                            {
                                var authorFaker = new Faker<Author>()
                                    .UseSeed(Seeder.DefaultSeed)
                                    .RuleFor(x => x.Name, f => f.Person.FullName);
                                var authorsToSeed = authorFaker.Generate(Seeder.DefaultAmount);
                                context.Set<Author>().AddRange(authorsToSeed);
                                await context.SaveChangesAsync(cancellationToken);
                            }

                            if (
                                !await context
                                    .Set<Book>()
                                    .AnyAsync(cancellationToken: cancellationToken)
                            )
                            {
                                var bookFaker = new Faker<Book>()
                                    .UseSeed(Seeder.DefaultSeed)
                                    .RuleFor(x => x.Name, f => f.Person.FullName)
                                    .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
                                    .RuleFor(x => x.Price, f => f.Random.Decimal(1, 100))
                                    .RuleFor(
                                        x => x.BookAuthors,
                                        f =>
                                            f.PickRandom(context.Set<Author>().Local)
                                                .Select(author => new BookAuthor(author.Id))
                                    )
                                    .RuleFor(
                                        x => x.BookCategories,
                                        f =>
                                            f.PickRandom(context.Set<Category>().Local)
                                                .Select(category => new BookCategory(category.Id))
                                    );
                                var booksToSeed = bookFaker.Generate(Seeder.DefaultLargeAmount);
                                context.Set<Book>().AddRange(booksToSeed);
                                await context.SaveChangesAsync(cancellationToken);
                            }
                        }
                    );
            }
        );

        builder.Services.AddScoped<IDatabaseFacade>(p => p.GetRequiredService<CatalogDbContext>());
        builder.Services.AddScoped(typeof(IReadRepository<>), typeof(CatalogRepository<>));
        builder.Services.AddScoped(typeof(IRepository<>), typeof(CatalogRepository<>));
    }
}
