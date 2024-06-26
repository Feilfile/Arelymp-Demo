using SharedLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class LeveledItem
    {
        public int AssignedItemId { get; set; }

        public int ItemLevel { get; set; }

        public int ItemExperience { get; set; }

        public bool IsMaxed { get; set; }

        public int EquippedTier { get; set; }

        public int ItemSchemaId { get; set; }

        public virtual ICollection<ItemSchema>? ItemSchema { get; set; }

        public virtual AssignedItem? AssignedItem { get; set; }

        /*public LeveledItem(int itemId, LevelPattern levelPattern) 
        {
            ItemId = itemId;
            ItemLevel = 1;
            ItemExperience = 0;
            LevelPattern = levelPattern;

            switch (LevelPattern)
            {
                case LevelPattern.Default:
                    MaxLevel = 4;
                    break;
            }
        }
        */
    }
}
