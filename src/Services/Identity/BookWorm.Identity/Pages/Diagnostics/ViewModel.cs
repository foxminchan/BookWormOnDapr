// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.Text;
using System.Text.Json;

namespace BookWorm.Identity.Pages.Diagnostics;

public sealed class ViewModel
{
    public ViewModel(AuthenticateResult result)
    {
        AuthenticateResult = result;

        if (result?.Properties?.Items.TryGetValue("client_list", out var encoded) == true)
        {
            if (encoded != null)
            {
                var bytes = Base64Url.Decode(encoded);
                var value = Encoding.UTF8.GetString(bytes);
                Clients = JsonSerializer.Deserialize<string[]>(value) ?? [];
                return;
            }
        }

        Clients = [];
    }

    public AuthenticateResult AuthenticateResult { get; }
    public IEnumerable<string> Clients { get; }
}
