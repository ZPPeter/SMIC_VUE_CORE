using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using SMIC.Authorization.Users;
using System;

namespace SMIC.Sessions.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        /*
        public string Name { get; set; }
        */

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }
                
        public  DateTime? ReadLastNoticeTime { get; set; } // LastLoginTime Abp内部无法读取

        public string[] Roles { get; set; }
    }
}
