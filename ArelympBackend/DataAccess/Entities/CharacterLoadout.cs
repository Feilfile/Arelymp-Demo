using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class CharacterLoadout
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public int CharacterId { get; set; }

        public int CharacterSkinId { get; set; }

        public int WeaponSkinId { get; set; }

        public virtual User? User { get; set; } = null;

        public virtual Item? CharacterSkin { get; set; } = null;

        public virtual Item? WeaponSkin { get; set; } = null;
    }
}
