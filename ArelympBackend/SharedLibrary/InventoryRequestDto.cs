using SharedLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLibrary
{
    // TODO: Receive userId from Token and remove this model
    public class InventoryRequestDto
    {
        public string userId { get; set; }

        public EquipSlot EquipSlot { get; set; }
    }
}
