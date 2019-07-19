using Abp.Authorization;
using Abp.Localization;

namespace MyPlugIn.Authorization
{
    public class PlugInZeroAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull("Pages") ?? context.CreatePermission("Pages", new FixedLocalizableString("Pages"));
            var mUsers = pages.CreateChildPermission(PlugInZeroConsts.PermissionNames.PlugIns, new FixedLocalizableString("插件测试"));
            mUsers.CreateChildPermission(PlugInZeroConsts.PermissionNames.PlugInZero, new FixedLocalizableString("插件测试子菜单"));
        }
   }
}
