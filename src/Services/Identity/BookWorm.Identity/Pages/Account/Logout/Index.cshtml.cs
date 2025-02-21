﻿// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using BookWorm.Identity.Models;

namespace BookWorm.Identity.Pages.Account.Logout;

[SecurityHeaders]
[AllowAnonymous]
public sealed class Index(
    SignInManager<ApplicationUser> signInManager,
    IIdentityServerInteractionService interaction,
    IEventService events
) : PageModel
{
    [BindProperty]
    public string? LogoutId { get; set; }

    public async Task<IActionResult> OnGet(string? logoutId)
    {
        LogoutId = logoutId;

        var showLogoutPrompt = LogoutOptions.ShowLogoutPrompt;

        if (User.Identity?.IsAuthenticated != true)
        {
            // if the user is not authenticated, then just show logged out page
            showLogoutPrompt = false;
        }
        else
        {
            var context = await interaction.GetLogoutContextAsync(LogoutId);
            if (context.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                showLogoutPrompt = false;
            }
        }

        if (showLogoutPrompt == false)
        {
            // if the request for logout was properly authenticated from IdentityServer, then
            // we don't need to show the prompt and can just log the user out directly.
            return await OnPost();
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return RedirectToPage("/Account/Logout/LoggedOut", new { logoutId = LogoutId });
        }

        // if there's no current logout context, we need to create one
        // this captures necessary info from the current logged-in user
        // this can still return null if there is no context needed
        LogoutId ??= await interaction.CreateLogoutContextAsync();

        // delete local authentication cookie
        await signInManager.SignOutAsync();

        // see if we need to trigger federated logout
        var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

        // raise the logout event
        await events.RaiseAsync(
            new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName())
        );
        Telemetry.Metrics.UserLogout(idp);

        // if it's a local login we can ignore this workflow
        if (idp is null or IdentityServerConstants.LocalIdentityProvider)
        {
            return RedirectToPage("/Account/Logout/LoggedOut", new { logoutId = LogoutId });
        }

        // we need to see if the provider supports external logout
        if (!await HttpContext.GetSchemeSupportsSignOutAsync(idp))
        {
            return RedirectToPage("/Account/Logout/LoggedOut", new { logoutId = LogoutId });
        }

        // build a return URL so the upstream provider will redirect back
        // to us after the user has logged out. this allows us to then
        // complete our single sign-out processing.
        var url = Url.Page("/Account/Logout/Loggedout", new { logoutId = LogoutId });

        // this triggers a redirect to the external provider for sign-out
        return SignOut(new AuthenticationProperties { RedirectUri = url }, idp);
    }
}
