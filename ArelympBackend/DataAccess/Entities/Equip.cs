using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Equip
    {
        public string UserId { get; set; }

        public int CharacterId { get; set; }

        public int CharacterSkinId { get; set; }

        public int WeaponSkinId { get; set; }

        public int? WeaponEffectId { get; set; }

        public int? AbilityOneSkinId { get; set; }

        public int? AbilityTwoSkinId { get; set; }

        public int? AbilityThreeSkinId { get; set; }

        public int? AbilityFourSkinId { get; set; }

        public int? ArmorEffectId { get; set; }

        public int? VictoryPoseId { get; set; }

        public int? TitleId { get; set; }

        public int? BannerId { get; set; }

        /* foreign relations */

        public virtual User? User { get; set; } = null;

        public virtual Item? Character { get; set; } = null;

        public virtual Item? CharacterSkin { get; set; } = null;

        public virtual Item? WeaponSkin { get; set; } = null;

        public virtual Item? WeaponEffect { get; set; } = null;

        public virtual Item? AbilityOneSkin { get; set; } = null;

        public virtual Item? AbilityTwoSkin { get; set; } = null;

        public virtual Item? AbilityThreeSkin { get; set; } = null;

        public virtual Item? AbilityFourSkin { get; set; } = null;

        public virtual Item? ArmorEffectSkin { get; set; } = null;

        public virtual Item? VictoryPoseSkin { get; set; } = null;

        public virtual Item? Title { get; set; } = null;

        public virtual Item? Banner { get; set; } = null;
    }
}
