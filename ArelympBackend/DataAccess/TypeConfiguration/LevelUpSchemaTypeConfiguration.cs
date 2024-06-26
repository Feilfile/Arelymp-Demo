using DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.TypeConfiguration
{
    internal class LevelUpSchemaTypeConfiguration : IEntityTypeConfiguration<LevelUpSchema>
    {
        public void Configure(EntityTypeBuilder<LevelUpSchema> entity)
        {
            entity.HasKey(e => new { e.ItemSchemaId, e.Level });

            entity.HasIndex(e => e.ItemSchemaId);
        }
    }
}
