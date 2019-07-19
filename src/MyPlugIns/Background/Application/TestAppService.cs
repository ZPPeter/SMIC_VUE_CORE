using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using Abp.Dapper.Repositories;
using System.Linq;
using System.Data;
using SMIC.PhoneBooks.Persons;
using DapperExtensions;
using DapperExtensions.Mapper;
using System.Reflection;
using System.Data.SqlClient;
using Dapper;
using DapperExtensions.Sql;
using SMIC;
using Microsoft.Extensions.Configuration;
using SMIC.Configuration;
using SMIC.Web;

namespace MyPlugIn.Application
{
    public class TestAppService : SMIC.SMICAppServiceBase
    {
        static IConfigurationRoot _appConfiguration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
        string connectionString = _appConfiguration.GetConnectionString(SMICConsts.ConnectionStringName);

        protected IDbConnection conn;  // Dapper 用
        protected IDatabase Db;        // DapperExtensions 用

        private readonly IDapperRepository<Person, int> _personDapperRepository;

        public TestAppService(IDapperRepository<Person, int> personDapperRepository)
        {
            _personDapperRepository = personDapperRepository;
        }


        public dynamic GetTestAbpDapper()  // 需要设置 SMIC.EntityFrameworkCore\EntityMapper\PersonMapper.cs
        {
            /*
            var a = 0;
            try
            {
                dynamic ret1 = 1 / a;
            }
            catch (System.Exception ex)
            {
                Logger.Error($"{GetType().FullName}:Logger详情测试!{ex}", ex);
            }
            */

            // Person -> PhoneNumbers
            dynamic ret = _personDapperRepository.GetAllPaged(x => x.Address != "SD.JN", 0, 10, "ID").ToDynamicList<dynamic>(); //OK
            return ret;
        }
    }
}
