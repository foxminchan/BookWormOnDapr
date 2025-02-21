// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace BookWorm.Identity.Pages.ExternalLogin;

[AllowAnonymous]
[SecurityHeaders]
public sealed class Challenge(IIdentityServerInteractionService interactionService) : PageModel
{
    public IActionResult OnGet(string scheme, string? returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = "~/";
        }

        // validate returnUrl - either it is a valid OIDC URL or back to a local page
        if (
            Url.IsLocalUrl(returnUrl) == false
            && interactionService.IsValidReturnUrl(returnUrl) == false
        )
        {
            // user might have clicked on a malicious link - should be logged
            throw new ArgumentException("invalid return URL");
        }

        // start challenge and roundtrip the return URL and scheme
        var props = new AuthenticationProperties
        {
            RedirectUri = Url.Page("/externallogin/callback"),
            Items = { { "returnUrl", returnUrl }, { "scheme", scheme } },
        };

        return Challenge(props, scheme);
    }
}
