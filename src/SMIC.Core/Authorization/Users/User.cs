using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.Extensions;

namespace SMIC.Authorization.Users
{
    public class User : AbpUser<User>  // ---> Abp.Authorization.Users
    {
        public const string DefaultPassword = "123qwe";
                
        //private DateTime? LastLoginTime2 { get; set; }
        
        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        // 修改 Name 可空
        private new string Name { get; set; }

        /*
        // SureName,EmailAddress 可为空
        private new string Surname { get; set; }

        [Required(AllowEmptyStrings = true)]
        public override string EmailAddress { get; set; }   // -> 可空
        */

        // 修改 Name，SureName,EmailAddress 可为空 - END
        // 将Name和SurName字段设置为private，这样设置之后，就有了值对象的感觉，EF在做实体验证的时候就不会对private的属性字段进行验证。
        // 而Emailaddress因为涉及了很多的业务情况，我们不能将它设置为私有访问，修改为 可空
        // 修改DbContext OnModelCreating
        // 修改 SMIC.Core\Authorization\Users\UserManager.cs
        // 在userManager领域服务中的CreateAsync提供的方法中，检查了EmailAddress 所以我们要重写方法。
        // 在 SMIC.Application\Users\Dto\CreateUserDto.cs 修改
        // 在 SMIC.Application\Authorization\Accounts\AccountAppService.cs Register 模块

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
