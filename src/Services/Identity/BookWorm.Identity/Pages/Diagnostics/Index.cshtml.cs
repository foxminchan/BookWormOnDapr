// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace BookWorm.Identity.Pages.Diagnostics;

[SecurityHeaders]
[Authorize]
public sealed class Index : PageModel
{
    public ViewModel View { get; set; } = null!;

    public async Task<IActionResult> OnGet()
    {
        var localAddresses = new List<string?> { "127.0.0.1", "::1" };
        if (HttpContext.Connection.LocalIpAddress is not null)
        {
            localAddresses.Add(HttpContext.Connection.LocalIpAddress.ToString());
        }

        if (!localAddresses.Contains(HttpContext.Connection.RemoteIpAddress?.ToString()))
        {
            return NotFound();
        }

        View = new(await HttpContext.AuthenticateAsync());

        return Page();
    }
}
