﻿
using DapperExtensions.Mapper;

namespace SMIC.Members
{
    public class AbpUserMapper : AutoClassMapper<AbpUser>
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
