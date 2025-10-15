using GymManagmentDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Data.Configurations
{
    public class GymUserConfig<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(X => X.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            builder.Property(X => X.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);
            builder.Property(X => X.Phone)
                .HasColumnType("varchar")
                .HasMaxLength(11);


            builder.OwnsOne(X => X.Address, ab =>
            {
                ab.Property(a => a.Street)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .HasColumnName("Street");

                ab.Property(a => a.City)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .HasColumnName("City");

                ab.Property(a => a.BuildingNumber)
                .HasColumnName("BuildingNumber");
                
            });

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserVaildEmailCheck", "Email Like '_%@_%._%'");
                Tb.HasCheckConstraint("GymUserVaildPhoneCheck", "Phone Like '01%' and phone not like '%[^0-9]%'");
            });
            builder.HasIndex(X => X.Email).IsUnique();
            builder.HasIndex(X => X.Phone).IsUnique();
            


        }
    }
}
