using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Auditing;
using SMIC.Sessions.Dto;

using System;
using System.Linq;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using SMIC.Members.Dto;

using System.Linq.Dynamic.Core;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Abp.Dapper.Repositories;

using Abp.Timing; // Clock.Now;
using Newtonsoft.Json;

using SMIC.Members;
using System.Linq.Expressions;
using SMIC.Utils;//Exceptionless;
using SMIC.Authorization.Roles;

using Abp.Notifications;
using Abp;

namespace SMIC.Sessions
{
    public class SessionAppService : SMICAppServiceBase, ISessionAppService
    {
        private readonly IDapperRepository<MemberUser, long> _memberDapperRepository;
        private readonly IDapperRepository<AbpUser, long> _userRepository;
        private readonly IDapperRepository<Role, int> _roleRepository;
        public SessionAppService(IDapperRepository<MemberUser, long> memberDapperRepository, IDapperRepository<Role, int> roleRepository, IDapperRepository<AbpUser, long> userRepository)
        {
            _memberDapperRepository = memberDapperRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        private void SetLastLoginTime()
        {
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
            _memberDapperRepository.Execute("update AbpUsers set LastLoginTime = '" + Clock.Now + "' where Id = " + AbpSession.UserId);
        }

        public void SetReadLastNoticeTime()
        {
            _memberDapperRepository.Execute("update AbpUsers set ReadLastNoticeTime = '" + Clock.Now + "' where Id = " + AbpSession.UserId);
        }

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

            output.Application.Features.Add("SignalR", true);
            output.Application.Features.Add("SignalR.AspNetCore", true);            

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
                output.User.Roles = GetRoles(output.User.Id);
                output.User.ReadLastNoticeTime = GetReadLastNoticeTime();
                SetLastLoginTime();
            }

            return output;
        }

        public string[] GetRoles(long userid)
        {
            var param = new { Id = userid };
            var ret = _roleRepository.Query("select b.NormalizedName from AbpRoles b where b.Id in (select RoleId from AbpUserRoles a where a.UserId = @Id)", param);

            List<string> list = new List<string>();
            foreach (Role r in ret)
                list.Add(r.NormalizedName);
            return list.ToArray();
        }

        private DateTime? GetReadLastNoticeTime() {
            return _userRepository.Get((long)AbpSession.UserId).ReadLastNoticeTime;
        }

    }
}
