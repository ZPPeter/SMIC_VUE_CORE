using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using SMIC.Authorization.Users;
using SMIC.MultiTenancy;

//using DapperExtensions;
using System.Data.SqlClient;
using Dapper;
namespace SMIC
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class SMICAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected SMICAppServiceBase()
        {
            LocalizationSourceName = SMICConsts.LocalizationSourceName;
        }

        protected virtual Task<User> GetCurrentUserAsync()
        {
            var user = UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            IDbConnection conn = new SqlConnection("connString");
            string query = "UPDATE Book SET  Name=@name WHERE id =@id";
            conn.Execute(query, book);

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
