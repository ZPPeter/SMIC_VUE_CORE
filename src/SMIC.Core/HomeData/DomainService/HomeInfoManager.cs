

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
using SMIC.HomeData;


namespace SMIC.HomeData.DomainService
{
    /// <summary>
    /// HomeInfo领域层的业务管理
    ///</summary>
    public class HomeInfoManager :SMICDomainServiceBase, IHomeInfoManager
    {
		
		private readonly IRepository<HomeInfo,int> _repository;

		/// <summary>
		/// HomeInfo的构造方法
		///</summary>
		public HomeInfoManager(
			IRepository<HomeInfo, int> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitHomeInfo()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
