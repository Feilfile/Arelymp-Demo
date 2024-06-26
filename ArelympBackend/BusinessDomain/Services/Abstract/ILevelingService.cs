using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessDomain.Services.Abstract
{
    public interface ILevelingService
    {
        LeveledItem AddItemLevelExperience(int rawAmount, LeveledItem item);
    }
}
