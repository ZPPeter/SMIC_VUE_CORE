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

using Abp.Auditing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyPlugIn.Application
{
    public class VueErrorLogAppService : SMICAppServiceBase // , IClientInfoProvider
    {
        //public string BrowserInfo => throw new System.NotImplementedException();
        //public string ClientIpAddress => throw new System.NotImplementedException();
        //public string ComputerName => throw new System.NotImplementedException();

        private readonly IClientInfoProvider _clientInfoProvider;

        public VueErrorLogAppService(IClientInfoProvider clientInfoProvider)
        {
            _clientInfoProvider = clientInfoProvider;
        }

        public void LoggerErr(Dto.VueErrorDto input)  // 需要设置 SMIC.EntityFrameworkCore\EntityMapper\PersonMapper.cs
        {
            /* 以下不适用于 .Net Core
            1、获取客户端IP：Request.ServerVariables.Get("Remote_Addr").ToString();
            2、获取客户端浏览器：Request.Browser.Browser;
            3、获取客户端浏览器 版本号：Request.Browser.MajorVersion;
            4、获取客户端操作系统：Request.Browser.Platform;
             */

            //Logger.Error(input.Detail);
            string clientIP = _clientInfoProvider.ClientIpAddress; // 获取不到？？？
            string clientBrowserInfo = _clientInfoProvider.BrowserInfo;
            string Ex = "【客户端错误】 " + clientIP + " , " + clientBrowserInfo + " , " + input.Detail;
            Logger.Error(Ex);
        }
    }
}
