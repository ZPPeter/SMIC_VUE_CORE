using System.Collections.Generic;
using Abp.Application.Services;

namespace SMIC.Enums
{
    /// <summary>
    /// 枚举信息的服务层
    /// </summary>
    public interface IEnumAppService : IApplicationService
    {


        List<KeyValuePair<string, string>> GetPhoneNumberTypeList();



    }
}