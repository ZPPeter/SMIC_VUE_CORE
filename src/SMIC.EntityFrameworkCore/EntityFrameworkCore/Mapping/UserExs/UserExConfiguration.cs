using SMIC.Authorization.Users;
using SMIC.Members;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SMIC.EntityFrameworkCore.Mapping.UserExs
{
    public class UserExConfiguration : EntityTypeConfiguration<AbpUser>
    {
        public override void Configure(EntityTypeBuilder<AbpUser> builder)
        {
            //设置一对多的关系 .Map()配置用于存储关系的外键列和表。
            /*
            Roles  HasMany此实体类型配置一对多关系。对应AbpUser实体               
            //WithMany   将关系配置为 many:many，且在关系的另一端有导航属性。
             * MapLeftKey 配置左外键的列名。左外键指向在 HasMany 调用中指定的导航属性的父实体。
             * MapRightKey 配置右外键的列名。右外键指向在 WithMany 调用中指定的导航属性的父实体。
             */
            builder.HasMany(x => x.Roles
        }
    }    
}
