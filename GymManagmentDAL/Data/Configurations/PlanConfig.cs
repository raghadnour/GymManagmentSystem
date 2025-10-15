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
    public class PlanConfig : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(X => X.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            builder.Property(X => X.Description)
                .HasColumnType("varchar")
                .HasMaxLength(200);
            builder.Property(X => X.Price)
                .HasColumnType("decimal(10,2)");
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("PlanDurationDaysCheck", "DurationDays BETWEEN 1 AND 365");
            });
        }

    }
}
