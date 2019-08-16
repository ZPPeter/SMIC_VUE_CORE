

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using SMIC.HomeData;


namespace SMIC.HomeData.DomainService
{
    public interface IHomeInfoManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitHomeInfo();



		 
      
         

    }
}
