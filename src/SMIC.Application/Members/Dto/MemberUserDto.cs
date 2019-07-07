using Abp.AutoMapper;
using SMIC.Users.Dto;

namespace SMIC.Members.Dto
{
    [AutoMapFrom(typeof(MemberUser))]
    public class MemberUserDto : UserDto
    {
        public string NickName { get; set; }

        public string HeadLogo { get; set; }

        public Gender Gender { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }
    }
}
