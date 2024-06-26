using SharedLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Item
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public EquipSlot EquipSlot { get; set; }

        public int? BindedCharacterId { get; set; }

        public int Cost { get; set; } // TODO: should be moved to another table

        public int? ItemSchemaId { get; set; }

        /* foreign relations */

        public virtual ICollection<Item>? BindedCharacter { get; set; }

        public virtual ItemSchema? ItemSchema { get; set; }

        public virtual ICollection<AssignedItem>? AssignedItems { get; set; } = null;
    }
}
