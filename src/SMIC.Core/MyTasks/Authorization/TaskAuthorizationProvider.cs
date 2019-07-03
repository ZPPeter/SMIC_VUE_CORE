

using System.Linq;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;
using SMIC.Authorization;

namespace SMIC.MyTasks.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="TaskPermissions" /> for all permission names. Task
    ///</summary>
    public class TaskAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

		public TaskAuthorizationProvider()
		{

		}

        public TaskAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public TaskAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

		public override void SetPermissions(IPermissionDefinitionContext context)
		{
			// 在这里配置了Task 的权限。
			var pages = context.GetPermissionOrNull(AppLtmPermissions.Pages) ?? context.CreatePermission(AppLtmPermissions.Pages, L("Pages"));

			var administration = pages.Children.FirstOrDefault(p => p.Name == AppLtmPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppLtmPermissions.Pages_Administration, L("Administration"));

			var entityPermission = administration.CreateChildPermission(TaskPermissions.Node , L("Task"));
			entityPermission.CreateChildPermission(TaskPermissions.Query, L("QueryTask"));
			entityPermission.CreateChildPermission(TaskPermissions.Create, L("CreateTask"));
			entityPermission.CreateChildPermission(TaskPermissions.Edit, L("EditTask"));
			entityPermission.CreateChildPermission(TaskPermissions.Delete, L("DeleteTask"));
			entityPermission.CreateChildPermission(TaskPermissions.BatchDelete, L("BatchDeleteTask"));
			entityPermission.CreateChildPermission(TaskPermissions.ExportExcel, L("ExportExcelTask"));


		}

		private static ILocalizableString L(string name)
		{
			return new LocalizableString(name, SMICConsts.LocalizationSourceName);
		}
    }
}