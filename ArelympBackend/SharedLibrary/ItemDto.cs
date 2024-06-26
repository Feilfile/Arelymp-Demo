using SharedLibrary.Enum;

namespace SharedLibrary
{
    public class ItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public EquipSlot EquipSlot { get; set; }

        public int? BindedCharacterId { get; set; }

        public int? ItemLevel { get; set; }

        public int Cost { get; set; }
    }
}
