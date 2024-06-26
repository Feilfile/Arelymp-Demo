using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.TypeConfiguration;

public class LeveledItemTypeConfiguration : IEntityTypeConfiguration<LeveledItem>
{
    public void Configure(EntityTypeBuilder<LeveledItem> builder)
    {
        builder.HasKey(b => b.AssignedItemId);

        builder.HasOne(b => b.AssignedItem)
               .WithOne(i => i.LeveledItem) // Assuming the Item entity has a navigation property to LeveledItem
               .HasForeignKey<LeveledItem>(b => b.AssignedItemId); // Assuming the FK is on the LeveledItem side


        /*builder
            .HasOne(n => n.Item)
            .WithOne(i => i.Level)
            .HasForeignKey(assignedItems => assignedItems.itemId);*/
    }
}
