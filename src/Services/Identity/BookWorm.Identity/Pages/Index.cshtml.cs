// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.Reflection;
using Duende.IdentityServer.Hosting;

namespace BookWorm.Identity.Pages.Home;

[AllowAnonymous]
public class Index : PageModel
{
    public Index(IdentityServerLicense? license = null)
    {
        License = license;
    }

    public string Version =>
        typeof(IdentityServerMiddleware)
            .Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion.Split('+')
            .First() ?? "unavailable";

    public IdentityServerLicense? License { get; }
}
