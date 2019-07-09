using System.ComponentModel.DataAnnotations;
using SMIC.Authorization.Users;
using System;
/*
 继承于User，并为之配置TPH关系
 扩充 User表
 Account/Register -> MemberUser  前台会员 , UserType = 1 UserConfiguration.cs
 User/Create                     后台用户 , UserType = 0
     public enum UserType : short
    {
        /// <summary>
        /// 后台用户
        /// </summary>
        Backend = 0,
        /// <summary>
        /// 前台会员
        /// </summary>
        Frontend = 1
    }

namespace SMIC.EntityFrameworkCore.Mapping.Users
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasDiscriminator<UserType>("UserType")
                .HasValue<User>(UserType.Backend)
                .HasValue<MemberUser>(UserType.Frontend);
        }
    }
}


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

        // 最近登录时间，ABP 已经去掉了
        public DateTime? LastLoginTime { get; set; }

    }
}

