

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using SMIC.MyTasks;


namespace SMIC.MyTasks.DomainService
{
    public interface ITaskManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitTask();



		 
      
         

    }
}
