using Abplus.ZeroDemo.Authorization.Users;
using Abplus.ZeroDemo.Members;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abplus.ZeroDemo.EntityFrameworkCore.Mapping.Users
{
    public class UserConfiguration : ZeroDemoEntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasDiscriminator<UserType>("UserType")
                .HasValue<User>(UserType.Backend)
                .HasValue<MemberUser>(UserType.Frontend);
        }
    }
}
