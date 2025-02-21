﻿namespace BookWorm.ServiceDefaults;

public sealed class Identity
{
    public string Url { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}
