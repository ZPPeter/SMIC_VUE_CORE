using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace SMIC.Authorization
{
    public class SMICAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // L("Users") 多语言设置
            /*
                <text name="Users">用户</text>
                <text name="Roles">角色</text>
                <text name="Tenants">租户</text>
             */
            context.CreatePermission(PermissionNames.Pages_Users, L("UsersManager"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("RolesManager"));
            //context.CreatePermission(PermissionNames.Pages_Tenants, L("TenantsManager"), multiTenancySides: MultiTenancySides.Host);
            //context.CreatePermission(PermissionNames.Pages_ChangeAvatar, L("ChangeAvatar"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SMICConsts.LocalizationSourceName);
        }
    }
}
