using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class CharacterLoadoutDto
    {
        public string? UserId { get; set; }

        public int CharacterId { get; set; }

        public int CharacterSkinId { get; set; }

        public int WeaponSkinId { get; set; }
    }
}
