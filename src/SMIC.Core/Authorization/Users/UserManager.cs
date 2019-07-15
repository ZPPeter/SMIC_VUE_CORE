using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using SMIC.Authorization.Roles;

namespace SMIC.Authorization.Users
{
    // private readonly IUnitOfWorkManager _unitOfWorkManager;

    public class UserManager : AbpUserManager<Role, User>
    {
        public UserManager(
            RoleManager roleManager,
            UserStore store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<User> passwordHasher, 
            IEnumerable<IUserValidator<User>> userValidators, 
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<UserManager<User>> logger, 
            IPermissionManager permissionManager, 
            IUnitOfWorkManager unitOfWorkManager, 
            ICacheManager cacheManager, 
            IRepository<OrganizationUnit, long> organizationUnitRepository, 
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository, 
            IOrganizationUnitSettings organizationUnitSettings, 
            ISettingManager settingManager)
            : base(
                roleManager, 
                store, 
                optionsAccessor, 
                passwordHasher, 
                userValidators, 
                passwordValidators, 
                keyNormalizer, 
                errors, 
                services, 
                logger, 
                permissionManager, 
                unitOfWorkManager, 
                cacheManager,
                organizationUnitRepository, 
                userOrganizationUnitRepository, 
                organizationUnitSettings, 
                settingManager)
        {
            // _unitOfWorkManager = unitOfWorkManager1;
        }

        /*
        public override async Task<IdentityResult> CheckDuplicateUsernameOrEmailAddressAsync(long? expectedUserId, string userName, string emailAddress)
        {
            var user = (await FindByNameAsync(userName));
            if (user != null && user.Id != expectedUserId)
            {
                return AbpIdentityResult.Failed(string.Format(L("Identity.DuplicateName"), userName));
            }

         
            return IdentityResult.Success;
        }

        private string L(string name)
        {
            return LocalizationManager.GetString(AbpZeroConsts.LocalizationSourceName, name);
        }

        public override async Task<IdentityResult> CreateAsync(User user)
        {
            var result = await CheckDuplicateUsernameOrEmailAddressAsync(user.Id, user.UserName, user.EmailAddress);
            if (!result.Succeeded)
            {
                return result;
            }

            user.EmailAddress = string.Empty;

            var tenantId = GetCurrentTenantId();
            if (tenantId.HasValue && !user.TenantId.HasValue)
            {
                user.TenantId = tenantId.Value;
            }

            try
            {
                return await base.CreateAsync(user);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
         
        }

        private int? GetCurrentTenantId()
        {
            if (_unitOfWorkManager.Current != null)
            {
                return _unitOfWorkManager.Current.GetTenantId();
            }

            return AbpSession.TenantId;
        }         
        */
    }
}
