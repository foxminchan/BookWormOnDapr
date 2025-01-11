using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;

namespace BookWorm.ServiceDefaults;

public static class OpenApiExtension
{
    public static IHostApplicationBuilder AddOpenApi(this IHostApplicationBuilder builder)
    {
        var openApi = builder.Configuration.GetSection(nameof(Document)).Get<Document>();

        if (openApi is null)
        {
            return builder;
        }

        string[] versions = ["v1"];
        foreach (var description in versions)
        {
            builder.Services.AddOpenApi(
                description,
                options =>
                {
                    options.ApplyApiVersionInfo(openApi.Title, openApi.Description);
                    options.ApplyOperationDeprecatedStatus();
                    options.ApplySchemaNullableFalse();
                    options.AddDocumentTransformer(
                        (document, context, cancellationToken) =>
                        {
                            document.Servers = [];
                            return Task.CompletedTask;
                        }
                    );
                }
            );
        }

        return builder;
    }

    public static IApplicationBuilder UseOpenApi(this WebApplication app)
    {
        var configuration = app.Configuration;
        var openApiSection = configuration.GetSection(nameof(Document)).Get<Document>();

        if (openApiSection is null)
        {
            return app;
        }

        app.MapOpenApi();

        if (app.Environment.IsDevelopment())
        {
            app.MapScalarApiReference(options =>
            {
                options.DefaultFonts = false;
            });
            app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();
        }

        return app;
    }
}
