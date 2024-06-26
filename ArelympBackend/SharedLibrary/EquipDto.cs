namespace SharedLibrary
{
    public class EquipDto
    {
        public int CharacterId { get; set; }

        public int CharacterSkinId { get; set; }

        public int CharacterSkinTier { get; set; }

        public int WeaponSkinId { get; set; }

        public int WeaponSkinTier { get; set; }

        public int? WeaponEffectId { get; set; }

        public int? AbilityOneSkinId { get; set; }

        public int? AbilityTwoSkinId { get; set; }

        public int? AbilityThreeSkinId { get; set; }

        public int? AbilityFourSkinId { get; set; }

        public int? ArmorEffectId { get; set; }

        public int? VictoryPoseId { get; set; }

        public int? TitleId { get; set; }

        public int? BannerId { get; set; }

        public ItemDto? Character { get; set; } = null;

        public ItemDto? CharacterSkin { get; set; } = null;

        public ItemDto? WeaponSkin { get; set; } = null;

        public ItemDto? WeaponEffect { get; set; } = null;

        public ItemDto? AbilityOneSkin { get; set; } = null;

        public ItemDto? AbilityTwoSkin { get; set; } = null;

        public ItemDto? AbilityThreeSkin { get; set; } = null;

        public ItemDto? AbilityFourSkin { get; set; } = null;

        public ItemDto? ArmorEffectSkin { get; set; } = null;

        public ItemDto? VictoryPoseSkin { get; set; } = null;

        public ItemDto? Title { get; set; } = null;

        public ItemDto? Banner { get; set; } = null;
    }
}