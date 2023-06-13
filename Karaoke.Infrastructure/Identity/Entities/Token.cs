﻿using Karaoke.Core.Entities.Common;

namespace Karaoke.Infrastructure.Identity.Entities;

public sealed class Token : AuditableEntity
{
    public string AccessToken { get; set; } = string.Empty;

    public string IpAddress { get; set; } = string.Empty;

    public int UsageCount { get; set; } = 0;

    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow;

    public string? UserId { get; set; }

    public ApplicationUser? User { get; set; }

    public int ExpiresIn => (int)ExpiresAt.Subtract(DateTime.UtcNow).TotalSeconds;

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
}