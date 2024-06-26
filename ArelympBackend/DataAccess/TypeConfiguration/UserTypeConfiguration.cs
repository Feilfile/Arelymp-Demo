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
    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .HasOne(user => user.Equip)
                .WithOne(inventory => inventory.User)
                .HasForeignKey<User>(user => user.Id);

            builder
                .HasMany(user => user.AssignedItems)
                .WithOne(assignedItems => assignedItems.User)
                .HasForeignKey(assignedItems => assignedItems.UserId);
        }
    }
}
