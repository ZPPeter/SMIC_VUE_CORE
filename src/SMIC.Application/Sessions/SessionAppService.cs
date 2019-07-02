using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Auditing;
using SMIC.Sessions.Dto;

/*
using SMIC.Configuration;
using SMIC.Web;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using SMIC.EntityFrameworkCore;
using Abp.Timing; // Clock.Now;
*/

namespace SMIC.Sessions
{
    public class SessionAppService : SMICAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>()
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            }

            /*
            // Add LastLoginTime
            // 还需要重写 GetAll
            //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());
            var builder = new DbContextOptionsBuilder<SMICDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
            //SMICDbContextConfigurer.Configure(builder, configuration.GetConnectionString(SMICConsts.ConnectionStringName));
            SMICDbContextConfigurer.Configure(builder, "Server=DEEP-1704241155; Database=SMICDbVue3; Trusted_Connection=True;"); //
            var context = new SMICDbContext(builder.Options);
            SqlParameter[] parameters = new[]{
                new SqlParameter("Id", AbpSession.UserId ),
                new SqlParameter("LastLoginTime", Clock.Now)
            };
            context.Database.ExecuteSqlCommand("update AbpUsers set LastLoginTime=@LastLoginTime where Id=@Id", parameters);            
            //Logger.Info(user.ToString());
            */

            return output;
        }
    }
}
