using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Auditing;
using SMIC.Sessions.Dto;

/*
using DapperExtensions;
using System.Data.SqlClient;
using Dapper;
using Abp.Dapper.Repositories;
*/

namespace SMIC.Sessions
{
    public class SessionAppService : SMICAppServiceBase, ISessionAppService
    {

        /*
        private readonly IDapperRepository<Person> _personDapperRepository;        
        protected SessionAppService(IDapperRepository<Person> personDapperRepository)
        {
            _personDapperRepository = personDapperRepository;
        }
        */
        //var people = _personDapperRepository.Query("select * from Persons");

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
            
            //var user = UserManager.FindByIdAsync(AbpSession.UserId.ToString());            
            //Logger.Info(AbpSession.UserId.ToString());

            return output;
        }
    }
}
