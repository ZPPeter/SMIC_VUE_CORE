using SMIC.Authorization.Users;
using SMIC.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SMIC.EntityFrameworkCore.Mapping.Users
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasDiscriminator<UserType>("UserType")
                .HasValue<User>(UserType.Backend)
                .HasValue<MemberUser>(UserType.Frontend);
        }
    }
}
