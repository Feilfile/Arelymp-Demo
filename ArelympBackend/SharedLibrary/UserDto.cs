namespace SharedLibrary
{
    public class UserDto
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string? Name { get; set; }

        public int Elo { get; set; }

        public int Level { get; set; }

        public int Experience { get; set; }

        public EquipDto? Equip { get; set; }
    }
}
