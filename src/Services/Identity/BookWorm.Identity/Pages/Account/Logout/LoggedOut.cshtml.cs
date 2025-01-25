// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace BookWorm.Identity.Pages.Account.Logout;

[SecurityHeaders]
[AllowAnonymous]
public sealed class LoggedOut(IIdentityServerInteractionService interactionService) : PageModel
{
    public LoggedOutViewModel View { get; set; } = null!;

    public async Task OnGet(string? logoutId)
    {
        // get context information (client name, post logout redirect URI and iframe for federated sign-out)
        var logout = await interactionService.GetLogoutContextAsync(logoutId);

        View = new()
        {
            AutomaticRedirectAfterSignOut = LogoutOptions.AutomaticRedirectAfterSignOut,
            PostLogoutRedirectUri = logout.PostLogoutRedirectUri,
            ClientName = string.IsNullOrEmpty(logout.ClientName)
                ? logout.ClientId
                : logout.ClientName,
            SignOutIframeUrl = logout.SignOutIFrameUrl,
        };
    }
}
