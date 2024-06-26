using BusinessDomain.Services.Abstract;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.Extensions.Caching.Memory;
using SharedLibrary.Enum;
using System.Collections.Concurrent;

namespace BusinessDomain.Services;

public class LevelingService : ILevelingService
{
    private readonly GameDbContext _dbContext;

    private readonly IMemoryCache _memoryCache;

    public LevelingService(GameDbContext dbContext, IMemoryCache memoryCache) 
    { 
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }

    public LeveledItem AddItemLevelExperience(int rawAmount, LeveledItem item)
    {
        if (item.IsMaxed) { return item; }

        var itemSchema = GetItemSchema(item.ItemSchemaId);
        var levelUpSchema = itemSchema.Levels.FirstOrDefault(l => l.Level == item.ItemLevel);

        item.ItemExperience += rawAmount;

        if (item.ItemExperience >= levelUpSchema.ExpRequired)
        {
            item.ItemExperience -= levelUpSchema.ExpRequired;
            item.ItemLevel++;

            if (item.ItemLevel == itemSchema.Levels.Count)
            {
                item.IsMaxed = true;
            }
        }

        return item;    
    }

    private ItemSchema GetItemSchema(int itemSchemaId)
    {
        ItemSchema? itemSchema;

        if (!_memoryCache.TryGetValue(itemSchemaId, out itemSchema))
        {
            itemSchema =_dbContext.ItemSchema
                .Include(i => i.Levels)
                .FirstOrDefault(i => i.Id == itemSchemaId);

            var cacheEntryOPtions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromDays(1));

            _memoryCache.Set(itemSchemaId, itemSchema, cacheEntryOPtions);
        }

        if (itemSchema == null)
        {
            throw new ArgumentNullException($"item schema with the id {itemSchemaId} does not exist");
        }

        return itemSchema;
    }
}
