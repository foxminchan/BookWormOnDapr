using BookWorm.Constants;
using BookWorm.Identity.Configurations;
using BookWorm.Identity.Data;
using BookWorm.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;

namespace BookWorm.Identity;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var appSettings = new AppSettings();

        builder.Configuration.Bind(appSettings);

        builder.AddServiceDefaults();

        builder.Services.AddRazorPages();

        builder.Services.AddMigration<ApplicationDbContext>();

        builder.AddNpgsqlDbContext<ApplicationDbContext>(
            ServiceName.Database.Identity,
            configureDbContextOptions: dbContextOptionsBuilder =>
                dbContextOptionsBuilder
                    .UseNpgsql()
                    .ConfigureWarnings(warnings =>
                        warnings.Ignore(RelationalEventId.PendingModelChangesWarning)
                    )
        );

        builder
            .Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        var identityServerBuilder = builder
            .Services.AddIdentityServer(options =>
            {
                options.Authentication.CookieLifetime = TimeSpan.FromHours(2);
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;

                if (builder.Environment.IsDevelopment())
                {
                    options.KeyManagement.Enabled = false;
                }
            })
            .AddInMemoryIdentityResources(Config.GetResources())
            .AddInMemoryApiScopes(Config.GetApiScopes())
            .AddInMemoryApiResources(Config.GetApis())
            .AddInMemoryClients(Config.GetClients(appSettings.Services))
            .AddAspNetIdentity<ApplicationUser>();

        // not recommended for production - you need to store your key material somewhere secure
        if (!builder.Environment.IsProduction())
        {
            identityServerBuilder.AddDeveloperSigningCredential();
        }

        builder.Services.AddAuthentication();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCookiePolicy(new() { MinimumSameSitePolicy = SameSiteMode.Lax });

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.MapDefaultEndpoints();

        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
