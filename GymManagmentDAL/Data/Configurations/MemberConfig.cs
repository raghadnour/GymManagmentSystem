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
    public class MemberConfig :GymUserConfig<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {

            builder.Property(X =>X.CreatedAt)
                .HasColumnName("JoinDate")
                .HasDefaultValueSql("getdate()");

            base.Configure(builder);
        }
    }
}
