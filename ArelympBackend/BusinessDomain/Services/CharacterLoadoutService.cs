using BusinessDomain.Extensions;
using BusinessDomain.Services.Abstract;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using SharedLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Services
{
    public class CharacterLoadoutService : ICharacterLoadoutService
    {
        private readonly GameDbContext _dbContext;

        public CharacterLoadoutService(GameDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CharacterLoadoutDto> GetCharacterLoadoutAsync(GameDbContext dbContext, string userId,
            int newCharacterId,
            CancellationToken cancellationToken)
        {
            var loadout = await dbContext.CharacterLoadout
                .Where(cl => cl.UserId == userId && cl.CharacterId == newCharacterId)
                .SingleOrDefaultAsync(cancellationToken);

            if (loadout == null)
            {
                var defaultLoadout = await dbContext.CharacterLoadout
                    .Include(l => l.CharacterSkin)
                    .Include(l => l.WeaponSkin)
                    .FirstOrDefaultAsync(u => u.UserId == null
                    && u.CharacterId == newCharacterId, cancellationToken);

                loadout = new CharacterLoadout
                {
                    UserId = userId,
                    CharacterId = newCharacterId,
                    CharacterSkinId = defaultLoadout.CharacterSkinId,
                    WeaponSkinId = defaultLoadout.WeaponSkinId
                };

                await dbContext.CharacterLoadout.AddAsync(loadout, cancellationToken);

                if (defaultLoadout == null || defaultLoadout.CharacterSkin == null || defaultLoadout.CharacterSkin.ItemSchemaId == null)
                {
                    throw new NullReferenceException($"Default character skin not found, id:{defaultLoadout.CharacterSkinId}");
                }

                if (defaultLoadout.WeaponSkin == null || defaultLoadout.WeaponSkin.ItemSchemaId == null)
                {
                    throw new NullReferenceException($"Default weapon skin not found, id:{defaultLoadout.CharacterSkinId}");
                }

                var skinAssign = new LeveledItem
                {
                    ItemLevel = 1,
                    ItemExperience = 0,
                    ItemSchemaId = defaultLoadout.CharacterSkin.ItemSchemaId.Value,
                    IsMaxed = false,
                    EquippedTier = 1,
                    AssignedItem = new AssignedItem
                    {
                        UserId = userId,
                        ItemId = loadout.CharacterSkinId,
                        EquipSlot = EquipSlot.CharacterSkin
                    }
                };

                var weaponAssign = new LeveledItem
                {
                    ItemLevel = 1,
                    ItemExperience = 0,
                    ItemSchemaId = defaultLoadout.WeaponSkin.ItemSchemaId.Value,
                    IsMaxed = false,
                    EquippedTier = 1,
                    AssignedItem = new AssignedItem
                    {
                        UserId = userId,
                        ItemId = loadout.WeaponSkinId,
                        EquipSlot = EquipSlot.WeaponSkin
                    }
                };

                await dbContext.LeveledItem.AddAsync(skinAssign, cancellationToken);
                await dbContext.LeveledItem.AddAsync(weaponAssign, cancellationToken);
                // Assigned Items get referenced afterwards

                try
                {
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    var assignedItems = dbContext.AssignedItem
                        .Where(a => a.UserId == userId &&
                    a.ItemId == loadout.CharacterSkinId || a.ItemId == loadout.WeaponSkinId);
                    
                    if (assignedItems.Count() != 2)
                    {
                        throw new Exception($"Data inconsistency: {userId} on Character with the id {newCharacterId}");
                    }
                }
            }

            return loadout.ToCharacterLoadoutDto();
        }

        public async Task UpdateCharacterSkinAsync(DbContext dbContext, string userId, int characterId, int characterSkinId, CancellationToken cancellationToken)
        {
            var loadout = await _dbContext.CharacterLoadout
                .Where(cl => cl.UserId == userId && cl.CharacterId == characterId)
                .SingleOrDefaultAsync(cancellationToken);

            if (loadout == null)
            {
                throw new NullReferenceException("Character loadout not found");
            }

            loadout.CharacterSkinId = characterSkinId;
            dbContext.Update(loadout);
        }

        public async Task UpdateWeaponSkinAsync(DbContext dbContext, string userId, int characterId, int weaponSkinId, CancellationToken cancellationToken)
        {
            var loadout = await _dbContext.CharacterLoadout
                .Where(cl => cl.UserId == userId && cl.CharacterId == characterId)
                .SingleOrDefaultAsync(cancellationToken);

            if (loadout == null)
            {
                throw new NullReferenceException("Character loadout not found");
            }

            loadout.WeaponSkinId = weaponSkinId;
            dbContext.Update(loadout);
        }

        #region helpers

        private async Task<CharacterLoadout> GetNewLoadoutFromDefaultAsync(string userId, int characterId, CancellationToken cancellationToken)
        {
            var defaultLoadout = await _dbContext.CharacterLoadout.FirstOrDefaultAsync(u => u.UserId == null
            && u.CharacterId == characterId, cancellationToken);

            return new CharacterLoadout
            {
                UserId = userId,
                CharacterId = characterId,
                CharacterSkinId = defaultLoadout.CharacterSkinId,
                WeaponSkinId = defaultLoadout.WeaponSkinId
            };
        }

        #endregion
    }
}
