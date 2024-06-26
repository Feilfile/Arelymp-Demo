    using DataAccess.Entities;
using DataAccess.TypeConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SharedLibrary;

namespace DataAccess
{
    public class GameDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public GameDbContext(DbContextOptions<GameDbContext> options, IConfiguration configuration) : base()
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                optionsBuilder.UseNpgsql(
                    connectionString,
                    options => options.MigrationsAssembly("ArelympApi")
                );
            }
        }

        public DbSet<User> Users { get; set; }

        public DbSet<AssignedItem> AssignedItem { get; set; }

        public DbSet<Equip> Equip { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<CharacterLoadout> CharacterLoadout { get; set; }

        public DbSet<LeveledItem> LeveledItem { get; set; }

        public DbSet<ItemSchema> ItemSchema { get; set; }

        public DbSet<LevelUpSchema> LevelUpSchema { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EquipTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AssignedItemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ItemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LeveledItemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CharacterLoadoutTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ItemSchemaTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LevelUpSchemaTypeConfiguration());
        }
    }
}
