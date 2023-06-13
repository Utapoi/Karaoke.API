﻿using Karaoke.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Karaoke.Infrastructure.Persistence.Configurations;

/// <summary>
///     ///     Configuration for <see cref="User" /> entity.
/// </summary>
public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // No configuration for now.
    }
}