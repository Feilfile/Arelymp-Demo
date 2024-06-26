using DataAccess.Entities;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Extensions
{
    public static class LoadoutExtensions
    {
        public static CharacterLoadoutDto ToCharacterLoadoutDto(this CharacterLoadout characterLoadout)
        {
            return new CharacterLoadoutDto
            {
                UserId = characterLoadout.UserId,
                CharacterId = characterLoadout.CharacterId,
                CharacterSkinId = characterLoadout.CharacterSkinId,
                WeaponSkinId = characterLoadout.WeaponSkinId
            };
        }
    }
}
