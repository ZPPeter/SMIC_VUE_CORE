using SMIC.Authorization.Users;
using SMIC.Members;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SMIC.EntityFrameworkCore.Mapping.UserExs
{
    public class UserExConfiguration : EntityTypeConfiguration<UserEx>
    {
        public override void Configure(EntityTypeBuilder<UserEx> builder)
        {
            builder.HasBaseType<User>();
        }
    }
}
