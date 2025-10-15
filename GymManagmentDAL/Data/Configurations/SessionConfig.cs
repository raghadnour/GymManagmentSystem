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
    public class SessionConfig : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("SessionCapacityCheck", "Capacity Between 1 and 25");
                Tb.HasCheckConstraint("SessionEndDateCheck", "EndTime > StartTime");
            });

            builder.HasOne(X => X.Category)
                     .WithMany(c => c.Sessions)
                     .HasForeignKey(X => X.CategoryId);
            builder.HasOne(X => X.SessionTrainer)
                        .WithMany(t => t.TrainerSessions)
                        .HasForeignKey(X => X.TrainerId);
        }
    }
}
