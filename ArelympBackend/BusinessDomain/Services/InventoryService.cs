using BusinessDomain.Extensions;
using BusinessDomain.Services.Abstract;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using SharedLibrary.Enum;
using System.CodeDom;

namespace BusinessDomain.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly GameDbContext _dbContext;

        private readonly ICharacterLoadoutService _characterLoadoutService;

        public InventoryService(GameDbContext dbContext, ICharacterLoadoutService characterLoadoutService) 
        {
            _dbContext = dbContext;
            _characterLoadoutService = characterLoadoutService;
        }

        private IQueryable<AssignedItem> GetAllAssignedItems()
        {
            return _dbContext.AssignedItem
                .Include(i => i.Item)
                .Include(i => i.LeveledItem);
        }

        public async Task<EquipDto> GetEquipOfUser(string userId, CancellationToken cancellationToken)
        {
            var equip = await _dbContext.Equip
                .Include(e => e.Character)
                .Include(e => e.CharacterSkin)
                .Include(e => e.WeaponSkin)
                .Include(e => e.Banner)
                .SingleOrDefaultAsync(e => e.UserId == userId, cancellationToken);

            if (equip == null)
            {
                throw new ArgumentNullException();
            }

            var equipedSkin = _dbContext.AssignedItem
                .Include(a => a.LeveledItem)
                .SingleOrDefault(a => a.UserId == userId && a.ItemId == equip.CharacterSkinId)!;

            var equipedWeapon = _dbContext.AssignedItem
                .Include(a => a.LeveledItem)
                .SingleOrDefault(a => a.UserId == userId && a.ItemId == equip.WeaponSkinId)!;

            var equipedSkinTier = equipedSkin.LeveledItem!.EquippedTier;
            var equipedWeaponTier = equipedWeapon.LeveledItem!.EquippedTier;

            return equip.ToEquipDto(equipedSkinTier, equipedWeaponTier, equipedSkin.LeveledItem.ItemLevel, equipedWeapon.LeveledItem.ItemLevel);
        }

        public async Task<IList<ItemDto?>> GetAssignedItemsOfSlot(string userId, EquipSlot slot, CancellationToken cancellationToken) 
        {
            return await GetAllAssignedItems()
                .Where(i => i.EquipSlot == slot && i.UserId == userId)
                .ToItemDto()
                .ToListAsync();
        }

        public async Task<IList<ItemDto?>> GetAllItems(string userId, CancellationToken cancellationToken)
        {
            return await GetAllAssignedItems()
                .Where(i => i.UserId == userId)
                .ToItemDto()
                .ToListAsync(cancellationToken);
        }

        public async Task<EquipDto?> EquipItem(string userId, int itemId, int? tier, CancellationToken cancellationToken)
        {
            var assigned = _dbContext.AssignedItem
                .Include(i => i.LeveledItem)
                .FirstOrDefault(i => 
                i.ItemId == itemId
                && i.UserId == userId);

            if (assigned == null)
            {
                throw new ArgumentNullException($"AssignedItem {itemId} of user {userId}");
            }

            var equipEntity = await _dbContext.Equip.SingleOrDefaultAsync(e => e.UserId == userId, cancellationToken);

            //if ( equipEntity == null ) { return null; }

            var item = await _dbContext.Items.SingleOrDefaultAsync(i => i.Id == itemId, cancellationToken);

            EquipDto? response = null;

            switch (item.EquipSlot)
            {
                case EquipSlot.Character:

                    var loadout = await _characterLoadoutService.GetCharacterLoadoutAsync(_dbContext, userId, item.Id, cancellationToken);

                    equipEntity.CharacterId = item.Id;
                    equipEntity.CharacterSkinId = loadout.CharacterSkinId;
                    equipEntity.WeaponSkinId = loadout.WeaponSkinId;

                    var charSkin = _dbContext.AssignedItem
                        .Include(a => a.Item)
                        .Include(a => a.LeveledItem)
                        .SingleOrDefault(a => a.UserId == userId && a.ItemId == equipEntity.CharacterSkinId)!;
                        
                    var equipedSkinTier = charSkin.LeveledItem!.EquippedTier;

                    var weaponSkin = _dbContext.AssignedItem
                        .Include(a => a.Item)
                        .Include(a => a.LeveledItem)
                        .SingleOrDefault(a => a.UserId == userId && a.ItemId == equipEntity.WeaponSkinId)!;

                    var equipedWeaponTier = weaponSkin.LeveledItem!.EquippedTier;

                    response = equipEntity.ToEquipDto(equipedSkinTier, equipedWeaponTier, charSkin.LeveledItem.ItemLevel, weaponSkin.LeveledItem.ItemLevel); //LAST TWO VALUES ARE OVERRIDDEN ANYWAYS

                    // Old Stale Items need to be overriden
                    response.Character = item.ToItemDto();
                    response.CharacterSkin = charSkin.Item!.ToItemDto(equipedSkinTier);
                    response.WeaponSkin = weaponSkin.Item!.ToItemDto(equipedWeaponTier);

                    break;

                case EquipSlot.CharacterSkin:

                    if (item.BindedCharacterId != equipEntity.CharacterId)
                    {
                        throw new ArgumentException("Character skin is not binded to character");
                    }

                    await _characterLoadoutService.UpdateCharacterSkinAsync(_dbContext, userId, equipEntity.CharacterId, item.Id, cancellationToken);
                    
                    //Skins must have a tier
                    if (assigned.LeveledItem!.ItemLevel < tier)
                    {
                        throw new Exception("The tier is not unlocked yet");
                    }

                    assigned.LeveledItem!.EquippedTier = tier!.Value;
                    equipEntity.CharacterSkinId = item.Id;

                    response = new EquipDto
                    {
                        CharacterSkinId = item.Id,
                        CharacterSkinTier = tier!.Value
                    };

                    break;

                case EquipSlot.WeaponSkin:

                    if (item.BindedCharacterId != equipEntity.CharacterId)
                    {
                        throw new ArgumentException("Weapon skin is not binded to character");
                    }

                    await  _characterLoadoutService.UpdateWeaponSkinAsync(_dbContext, userId, equipEntity.CharacterId, item.Id, cancellationToken);

                    //Skins must have a tier
                    if (assigned.LeveledItem!.ItemLevel < tier)
                    {
                        throw new Exception("The tier is not unlocked yet");
                    }

                    assigned.LeveledItem!.EquippedTier = tier!.Value;
                    equipEntity.WeaponSkinId = item.Id;

                    response = new EquipDto
                    {
                        CharacterSkinId = item.Id,
                        CharacterSkinTier = tier!.Value
                    };

                    break;

                case EquipSlot.WepaonEffect:
                    equipEntity.WeaponEffectId = item.Id;
                    break;

                case EquipSlot.AbilityOneSkin:
                    equipEntity.AbilityOneSkinId = item.Id;
                    break;

                case EquipSlot.AbilityTwoSkin:
                    equipEntity.AbilityTwoSkinId = item.Id;
                    break;

                case EquipSlot.AbilityThreeSkin:
                    equipEntity.AbilityThreeSkinId = item.Id;
                    break;

                case EquipSlot.AbilityFourSkin:
                    equipEntity.AbilityFourSkinId = item.Id;
                    break;

                case EquipSlot.ArmorEffect:
                    equipEntity.ArmorEffectId = item.Id;
                    break;

                case EquipSlot.VictoryPose:
                    equipEntity.VictoryPoseId = item.Id;
                    break;

                case EquipSlot.Title:
                    equipEntity.TitleId = item.Id;
                    break;

                case EquipSlot.Banner:
                    equipEntity.BannerId = item.Id;
                    break;

                default:
                    throw new KeyNotFoundException();
            }

            _ = _dbContext.Equip.Update(equipEntity);
            await _dbContext.SaveChangesAsync();
            
            return response;
        }

        public async Task AssignItemtoUser(Item item, string userId, CancellationToken cancellationToken)
        {
            var assignedItem = new AssignedItem
            {
                ItemId = item.Id,
                UserId = userId,
                EquipSlot = item.EquipSlot
            };

            if (item.ItemSchemaId == null)
            {
                await _dbContext.AssignedItem.AddAsync(assignedItem, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return;
            }

            var skinAssigned = new LeveledItem
            {
                ItemLevel = 1,
                ItemExperience = 0,
                ItemSchemaId = item.ItemSchemaId.Value,
                IsMaxed = false,
                AssignedItem = assignedItem,
            };

            await _dbContext.LeveledItem.AddAsync(skinAssigned, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
