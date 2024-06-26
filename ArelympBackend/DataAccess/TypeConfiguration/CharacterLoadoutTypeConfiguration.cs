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
    public class CharacterLoadoutTypeConfiguration : IEntityTypeConfiguration<CharacterLoadout>
    {
        public void Configure(EntityTypeBuilder<CharacterLoadout> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => new { e.UserId, e.CharacterId }).IsUnique();

            entity.Property(e => e.UserId)
                .IsRequired(false); 

            entity.HasOne(loadout => loadout.User)
                .WithMany()
                .HasForeignKey(loadout => loadout.UserId);

            entity.HasOne(loadout => loadout.CharacterSkin)
                .WithMany()
                .HasForeignKey(loadout => loadout.CharacterSkinId);

            entity.HasOne(loadout => loadout.WeaponSkin)
                .WithMany()
                .HasForeignKey(loadout => loadout.WeaponSkinId);
        }
    }
}
