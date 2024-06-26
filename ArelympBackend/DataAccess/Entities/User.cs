using SharedLibrary.Enum;
using System;
using System.Collections.ObjectModel;

namespace DataAccess.Entities
{
    public class User
    {
        public string Id { get; set; }

        //public Platform Platform { get; }

        public string Email { get; set; }

        public string Name { get; set; }

        public int Elo { get; set; }

        public int Level { get; set; }

        public int Experience { get; set; }

        /* foreign relations */

        public virtual Equip? Equip { get; set; } = null;

        public virtual ICollection<AssignedItem>? AssignedItems { get; set; } = null;
    }
}
