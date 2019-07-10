
using DapperExtensions.Mapper;
using SMIC.Members;
namespace SMIC.EntityFrameworkCore.EntityMapper
{
    public class AbpUserMapper : ClassMapper<AbpUser>
    {
        public AbpUserMapper()
        {
            Table("AbpUsers");
            Map(x => x.Roles).Ignore();
            AutoMap();
        }
    }
}
