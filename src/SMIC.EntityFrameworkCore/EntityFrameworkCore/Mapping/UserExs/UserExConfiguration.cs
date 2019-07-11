using SMIC.Authorization.Users;
using SMIC.Members;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SMIC.EntityFrameworkCore.Mapping.UserExs
{
    public class UserExConfiguration : EntityTypeConfiguration<AbpUser>
    {
        public override void Configure(EntityTypeBuilder<AbpUser> builder)
        {
            /* - 仅用于类似功能 - 
             * 设置一对多，主键外键, AutoMap 可自动完成，不用在此设置
            builder.HasDiscriminator<UserType>("UserType")
            .HasValue<User>(UserType.Backend)
            .HasValue<MemberUser>(UserType.Frontend);
            */
        }
    }    
}
