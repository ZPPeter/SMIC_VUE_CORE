

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Abp.UI;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

using SMIC;
using SMIC.MyTasks;


namespace SMIC.MyTasks.DomainService
{
    /// <summary>
    /// Task领域层的业务管理
    ///</summary>
    public class TaskManager :SMICDomainServiceBase, ITaskManager
    {
		
		private readonly IRepository<SMIC.MyTasks.MyTask, int> _repository;

		/// <summary>
		/// Task的构造方法
		///</summary>
		public TaskManager(
			IRepository<MyTask, int> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitTask()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
