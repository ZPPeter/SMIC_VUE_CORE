

namespace SMIC.MyTasks.Authorization
{
	/// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="TaskAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class TaskPermissions
	{
		/// <summary>
		/// Task权限节点
		///</summary>
		public const string Node = "Pages.Task";

		/// <summary>
		/// Task查询授权
		///</summary>
		public const string Query = "Pages.Task.Query";

		/// <summary>
		/// Task创建权限
		///</summary>
		public const string Create = "Pages.Task.Create";

		/// <summary>
		/// Task修改权限
		///</summary>
		public const string Edit = "Pages.Task.Edit";

		/// <summary>
		/// Task删除权限
		///</summary>
		public const string Delete = "Pages.Task.Delete";

        /// <summary>
		/// Task批量删除权限
		///</summary>
		public const string BatchDelete = "Pages.Task.BatchDelete";

		/// <summary>
		/// Task导出Excel
		///</summary>
		public const string ExportExcel="Pages.Task.ExportExcel";

		 
		 
         
    }

}

