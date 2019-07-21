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
using Abp.Runtime.Session;
using Abp.UI;
using SMIC.Common.Dto;
namespace SMIC.Common
{
    public class UploadAvatarAppService : SMICAppServiceBase
    {
        private readonly IAbpSession _abpSession;
        public UploadAvatarAppService(IAbpSession abpSession)
        {
            _abpSession = abpSession;
        }
                
        public string UploadFile(UploadAvatarDto input)
        {
            Logger.Error("Upload: ");
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("请登录后再进行操作！");
            }
            long userId = _abpSession.UserId.Value;
            Logger.Error("Upload: "+userId.ToString());
            //var files = HttpRequest.Form.Files;
            //HttpContext.Current.Request.Files

            //string Ex = "";
            //Logger.Error();
            return userId + ":" + input.File.Name;
        }
    }
}
