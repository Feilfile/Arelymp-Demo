using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class LevelUpSchema
    {
        public int ItemSchemaId { get; set; }

        public int Level { get; set; }

        public int ExpRequired { get; set; }

        public virtual ItemSchema ItemSchema { get; set; }
    }
}
