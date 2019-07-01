using System.ComponentModel.DataAnnotations;
using SMIC.Authorization.Users;
/*
 继承于User，并为之配置TPH关系
 扩充 User表
     */
namespace SMIC.Members
{
    public class MemberUser : User
    {
        public const int NickNameMaxLength = 64;

        public const int SessionKeyMaxLength = 256;
        public const int UnionIdMaxLength = 128;

        public const int CityMaxLength = 64;
        public const int ProvinceMaxLength = 64;
        public const int CountryMaxLength = 64;

        [MaxLength(UnionIdMaxLength)]
        public string UnionId { get; set; }

        [MaxLength(SessionKeyMaxLength)]
        public string SessionKey { get; set; }

        [MaxLength(NickNameMaxLength)]
        public string NickName { get; set; }

        public Gender Gender { get; set; }

        [MaxLength(CityMaxLength)]
        public string City { get; set; }

        [MaxLength(ProvinceMaxLength)]
        public string Province { get; set; }

        [MaxLength(CountryMaxLength)]
        public string Country { get; set; }
    }
}

