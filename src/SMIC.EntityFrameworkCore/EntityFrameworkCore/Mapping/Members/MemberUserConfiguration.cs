using SMIC.Authorization.Users;
using SMIC.Members;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SMIC.EntityFrameworkCore.Mapping.Members
{
    public class MemberUserConfiguration : EntityTypeConfiguration<MemberUser>
    {
        public override void Configure(EntityTypeBuilder<MemberUser> builder)
        {
            builder.HasBaseType<User>();
        }
    }
}
