﻿using Karaoke.Application.Persistence;
using Karaoke.Application.Tags;
using Karaoke.Core.Entities;

namespace Karaoke.Infrastructure.Tags;

public sealed class TagsService : ITagsService
{
    private readonly IKaraokeDbContext _context;

    public TagsService(IKaraokeDbContext context)
    {
        _context = context;
    }

    public Tag GetOrCreateByName(string name)
    {
        var tag = _context
            .Tags
            .FirstOrDefault(x => x.Name == name);

        if (tag is not null)
        {
            return tag;
        }

        tag = new Tag { Name = name };

        _context.Tags.Add(tag);
        _context.SaveChanges();

        return tag;
    }

    public Tag? GetById(Guid id)
    {
        return _context
            .Tags
            .FirstOrDefault(x => x.Id == id);
    }
}