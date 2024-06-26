using DataAccess.Entities;
using SharedLibrary;
using SharedLibrary.Enum;
using System.Text.RegularExpressions;

namespace BusinessDomain.Extensions
{
    public static class UserExtensions
    {
        public static string AddPlatformPrefix(this string userId, Platform platform)
        {
            return $"{platform}@{userId}";
        }

        public static string RemovePlatformPrefix(this string plaformUserId)
        {
            var pattern = "^.+?@";
            return Regex.Replace(plaformUserId, pattern, string.Empty);
        }

        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id.RemovePlatformPrefix(),
                Email = user.Email,
                Name = user.Name,
                Elo = user.Elo,
                Level = user.Level,
                Experience = user.Experience,
                Equip = user.Equip != null ? new EquipDto
                {
                    CharacterId = user.Equip.CharacterId,
                    CharacterSkinId = user.Equip.CharacterSkinId,
                    WeaponSkinId = user.Equip.WeaponSkinId,
                    WeaponEffectId = user.Equip.WeaponEffectId,
                    AbilityOneSkinId = user.Equip.AbilityOneSkinId,
                    AbilityTwoSkinId = user.Equip.AbilityTwoSkinId,
                    AbilityThreeSkinId = user.Equip.AbilityThreeSkinId,
                    AbilityFourSkinId = user.Equip.AbilityFourSkinId,
                    ArmorEffectId = user.Equip.ArmorEffectId,
                    VictoryPoseId = user.Equip.VictoryPoseId,
                    TitleId = user.Equip.TitleId,
                    BannerId = user.Equip.BannerId,
                } : null
            };
        }
    }
}
