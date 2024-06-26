using DataAccess.Entities;
using SharedLibrary;
using System.Runtime.InteropServices;

namespace BusinessDomain.Extensions
{
    public static class InventoryExtension
    {
        public static IQueryable<ItemDto?> ToItemDto(this IQueryable<AssignedItem?> it)
        {
            return it.Select(i => new ItemDto
            {
                Id = i.Item.Id,
                Name = i.Item.Name,
                EquipSlot = i.Item.EquipSlot,
                BindedCharacterId = i.Item.BindedCharacterId,
                Cost = i.Item.Cost,
                ItemLevel = i.LeveledItem == null ? null 
                    : i.LeveledItem.ItemLevel
            });
        }

        public static ItemDto ToItemDto(this Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                EquipSlot = item.EquipSlot,
                BindedCharacterId = item.BindedCharacterId,
                Cost = item.Cost
            };
        }

        public static ItemDto ToItemDto(this Item item, int itemLevel)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                EquipSlot = item.EquipSlot,
                BindedCharacterId = item.BindedCharacterId,
                Cost = item.Cost,
                ItemLevel = itemLevel
            };
        }

        public static EquipDto ToEquipDto(this Equip e, int equipedSkinTier, int equipedWeaponTier, int userSkinLevel, int userWeaponLevel)
        {
            return new EquipDto
            {
                CharacterId = e.CharacterId,
                Character = e.Character?.ToItemDto(),
                CharacterSkinId = e.CharacterSkinId,
                CharacterSkinTier = equipedSkinTier,
                CharacterSkin = e.CharacterSkin?.ToItemDto(userSkinLevel),
                WeaponSkinId = e.WeaponSkinId,
                WeaponSkinTier = equipedWeaponTier,
                WeaponSkin = e.WeaponSkin?.ToItemDto(userWeaponLevel),
                BannerId = e.BannerId,
                Banner = e.Banner?.ToItemDto()
            };
        }
    }
}
