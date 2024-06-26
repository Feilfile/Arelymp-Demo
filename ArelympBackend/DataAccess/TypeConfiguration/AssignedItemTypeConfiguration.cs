using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedLibrary;

namespace DataAccess.TypeConfiguration
{
    public class AssignedItemTypeConfiguration : IEntityTypeConfiguration<AssignedItem>
    {
        public void Configure(EntityTypeBuilder<AssignedItem> entity)
        {
            entity.HasKey(e => e.AssignedItemId); // Primary key

            entity.HasAlternateKey(e => new { e.UserId, e.ItemId }); // Alternate key

            entity.HasIndex(e => e.UserId);

            entity.HasOne(e => e.LeveledItem)
                .WithOne(e => e.AssignedItem)
                .HasForeignKey<AssignedItem>(e => e.AssignedItemId);
        }
    }
}
