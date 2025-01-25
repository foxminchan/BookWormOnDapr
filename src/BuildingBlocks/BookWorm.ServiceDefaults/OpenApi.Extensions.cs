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
        var identitySection = builder.Configuration.GetSection(nameof(Identity));

        var scopes = identitySection.Exists()
            ? identitySection
                .GetRequiredSection("Scopes")
                .GetChildren()
                .ToDictionary(p => p.Key, p => p.Value)
            : [];

        if (openApi is null)
        {
            return builder;
        }

        string[] versions = ["v1", "v2"];
        foreach (var description in versions)
        {
            builder.Services.AddOpenApi(
                description,
                options =>
                {
                    options.ApplyApiVersionInfo(openApi.Title, openApi.Description);
                    options.ApplyAuthorizationChecks([.. scopes.Keys]);
                    options.ApplySecuritySchemeDefinitions();
                    options.ApplyOperationDeprecatedStatus();
                    options.ApplySchemaNullableFalse();
                    options.AddDocumentTransformer(
                        (document, _, _) =>
                        {
                            document.Servers = [];
                            return Task.CompletedTask;
                        }
                    );
                }
            );
        }

        builder.Services.AddEndpointsApiExplorer();

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

        if (app.Environment.IsProduction())
        {
            return app;
        }

        app.MapScalarApiReference(options =>
        {
            options.Title = openApiSection.Title;
            options.DefaultFonts = false;

            var identitySection = configuration.GetSection(nameof(Identity)).Get<Identity>();

            if (identitySection is null)
            {
                return;
            }

            options.WithOAuth2Authentication(oauth2Options =>
            {
                oauth2Options.ClientId = identitySection.ClientId;
            });
        });

        app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();

        return app;
    }
}
