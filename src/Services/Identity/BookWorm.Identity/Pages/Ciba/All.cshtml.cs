// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace BookWorm.Identity.Pages.Ciba;

[SecurityHeaders]
[Authorize]
public sealed class AllModel(
    IBackchannelAuthenticationInteractionService backchannelAuthenticationInteractionService
) : PageModel
{
    public IEnumerable<BackchannelUserLoginRequest> Logins { get; set; } = null!;

    public async Task OnGet()
    {
        Logins =
            await backchannelAuthenticationInteractionService.GetPendingLoginRequestsForCurrentUserAsync();
    }
}
