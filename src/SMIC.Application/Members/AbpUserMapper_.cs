
using DapperExtensions.Mapper;
using SMIC.Members;
namespace SMIC.EntityFrameworkCore.EntityMapper
{
    public class AbpUserMapper : ClassMapper<AbpUser>
    {
        public AbpUserMapper()
        {
            Table("AbpUsers");
            Map(x => x.RoleNames).Ignore();
            //Map(x => x.Roles).Ignore(); //没法和 Role 一对多关联
            AutoMap();
        }
    }
}
