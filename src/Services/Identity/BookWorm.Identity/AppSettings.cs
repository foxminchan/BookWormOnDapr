﻿namespace BookWorm.Identity;

public sealed class AppSettings
{
    public ServiceOptions Services { get; set; } = new();
}
