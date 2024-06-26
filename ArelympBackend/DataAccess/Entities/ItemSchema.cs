using System.Collections.ObjectModel;

namespace DataAccess.Entities
{
    public class ItemSchema
    {
        public int Id { get; set; }

        //public int MaxLevel { get; set; }

        public virtual ICollection<LevelUpSchema> Levels { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
