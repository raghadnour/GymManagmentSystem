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
    internal class HealthRecordConfig : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members")
                .HasKey(X=>X.Id);

            builder.HasOne<Member>()
                .WithOne(M=>M.HealthRecord)
                .HasForeignKey<HealthRecord>(X=>X.Id);
            builder.Ignore(X => X.CreatedAt);
            builder.Ignore(X => X.UpdatedAt);

        }
    }
}
