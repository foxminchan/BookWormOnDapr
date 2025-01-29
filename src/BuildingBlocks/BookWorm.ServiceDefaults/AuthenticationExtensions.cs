using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BookWorm.ServiceDefaults;

public static class AuthenticationExtensions
{
    public static IHostApplicationBuilder AddDefaultAuthentication(
        this IHostApplicationBuilder builder
    )
    {
        var identitySection = builder.Configuration.GetSection(nameof(Identity)).Get<Identity>();

        if (identitySection is null)
        {
            return builder;
        }

        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        builder
            .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var identityUrl = identitySection.Url;

                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = identitySection.Audience;

                options.TokenValidationParameters.ValidIssuers = [identityUrl];
                options.TokenValidationParameters.ValidateAudience = false;
            });

        builder.Services.AddAuthorization();
        builder.Services.AddHttpContextAccessor();

        return builder;
    }
}
