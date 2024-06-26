using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.TypeConfiguration
{
    public class EquipTypeConfiguration : IEntityTypeConfiguration<Equip>
    {
        public void Configure(EntityTypeBuilder<Equip> builder)
        {
            builder.HasKey(b => b.UserId);

            builder
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.Character)
                .WithMany()
                .HasForeignKey(b => b.CharacterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.CharacterSkin)
                .WithMany()
                .HasForeignKey(b => b.CharacterSkinId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.WeaponSkin)
                .WithMany()
                .HasForeignKey(b => b.WeaponSkinId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.WeaponEffect)
                .WithMany()
                .HasForeignKey(b => b.WeaponEffectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.AbilityOneSkin)
                .WithMany()
                .HasForeignKey(b => b.AbilityOneSkinId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.AbilityTwoSkin)
                .WithMany()
                .HasForeignKey(b => b.AbilityTwoSkinId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.AbilityThreeSkin)
                .WithMany()
                .HasForeignKey(b => b.AbilityThreeSkinId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.AbilityFourSkin)
                .WithMany()
                .HasForeignKey(b => b.AbilityFourSkinId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.ArmorEffectSkin)
                .WithMany()
                .HasForeignKey(b => b.ArmorEffectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.VictoryPoseSkin)
                .WithMany()
                .HasForeignKey(b => b.VictoryPoseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.Title)
                .WithMany()
                .HasForeignKey(b => b.TitleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(b => b.Banner)
                .WithMany()
                .HasForeignKey(b => b.BannerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
