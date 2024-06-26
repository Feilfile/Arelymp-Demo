using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.TypeConfiguration
{
    public class ItemTypeConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(b => b.Id);

            builder
                .HasMany(i => i.AssignedItems)
                .WithOne(assignedItems => assignedItems.Item)
                .HasForeignKey(assignedItems => assignedItems.ItemId);

            builder.HasMany(i => i.BindedCharacter)
                .WithOne()
                .HasForeignKey(i => i.BindedCharacterId);
        }
    }
}
