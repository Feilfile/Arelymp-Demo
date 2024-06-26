using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.TypeConfiguration
{
    internal class ItemSchemaTypeConfiguration : IEntityTypeConfiguration<ItemSchema>
    {
        public void Configure(EntityTypeBuilder<ItemSchema> entity)
        {
            entity.HasKey(x => x.Id);

            entity.HasMany(x => x.Levels)
                .WithOne(x => x.ItemSchema)
                .HasForeignKey(x => x.ItemSchemaId);

            entity.HasMany(x => x.Items)
                .WithOne(x => x.ItemSchema)
                .HasForeignKey(x => x.ItemSchemaId);
        }
    }
}
