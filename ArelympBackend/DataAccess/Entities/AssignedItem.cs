using SharedLibrary.Enum;

namespace DataAccess.Entities
{
    public class AssignedItem
    {
        public int AssignedItemId { get; set; }

        public string UserId { get; set; }

        public int ItemId { get; set; }

        public EquipSlot EquipSlot { get; set; }

        /* foreign relations*/

        public virtual User? User { get; set; } = null;

        public virtual Item? Item { get; set; } = null;

        public virtual LeveledItem? LeveledItem { get; set; } = null;

        /*public AssignedItem() 
        {
            if (EquipSlot is EquipSlot.WeaponSkin
                || EquipSlot is EquipSlot.CharacterSkin)
            {
                ItemLevel = 0;
                ItemExperience = 0;
            }
            else
            {
                ItemLevel = null;
                ItemExperience = null;
            }
        }*/
    }
}
