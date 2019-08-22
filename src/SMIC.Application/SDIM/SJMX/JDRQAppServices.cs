﻿using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Abp.Dapper.Repositories;
using System.Linq.Expressions;
using System.Linq;
using System;
using Abp.Domain.Entities;
using Abp.Runtime.Caching;
using Abp.Domain.Uow;

namespace SMIC.SDIM
{
    public class JDRQAppServices : SMICAppServiceBase
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDapperRepository<JDRQ, long> _jdrqDapperRepository;
        private readonly ICacheManager _cacheManager;//依赖注入缓存
        private readonly SJMXAppServices _AppServices;
        public JDRQAppServices(IUnitOfWorkManager unitOfWorkManager, IDapperRepository<JDRQ, long> jdrqDapperRepository, ICacheManager cacheManager, SJMXAppServices AppServices)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _jdrqDapperRepository = jdrqDapperRepository;
            _cacheManager = cacheManager; // 依赖注入缓存
            _AppServices = AppServices;
        }

        public int Add(int id, string jdy) //string jdy = (Int32.Parse(context.Request.Form["jdy"].ToString())-100000).ToString();
        {
            int ret = 0;
            string strSQL = "select top 1 ID from  YQSF_DPII_JDRQ where ID=" + id;
            if (_jdrqDapperRepository.Query(strSQL).Count()==0) //确保不存在再 插入
            {                
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    strSQL = "insert into YQSF_DPII_JDRQ(id,jdrq,jdzt,jdy) values(" + id + ",'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'," + 100 + ",'" + jdy + "')";
                    _jdrqDapperRepository.Execute(strSQL);
                    strSQL = "update YQSF_SJD set qzyjdzt = '正在检定' where ID = (select sjdid from YQSF_SJMX as b where b.id = " + id + ")";
                    _jdrqDapperRepository.Execute(strSQL);
                    unitOfWork.Complete();
                    ret = 1;
                }
            }
            return ret;
        }

        
        // 检定完毕
        // 122 - 待核验
        // 222 - 全部检完
        public void SetOverByJdzt(int id, int jdzt)
        {
            string strSQL = "update YQSF_DPII_JDRQ set jdzt = " + jdzt + ",jwrq='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "' where id = " + id;
            _jdrqDapperRepository.Execute(strSQL);
            _AppServices.CheckSjmxJdzt(id);
        }
        

        /// <summary>
        /// 200
        /// 222
        /// </summary>
        /// <param name="id"></param>
        /// <param name="jdzt"></param>
        /// <returns></returns>
        public void SetOver(int id, int jdzt, string p, string yj)
        {
            string strSQL = "";
            if (jdzt == 200)
                strSQL = "update YQSF_DPII_JDRQ set jdzt = 200,hyy='" + p + "',hyyj='" + yj + "' where id = " + id;
            else if (jdzt == 222 && id != 9)
                strSQL = "update YQSF_DPII_JDRQ set jdzt = 222,pzr='" + p + "',pzyj='批准' where id = " + id;
            else if (jdzt == 222 && id == 9)
                strSQL = "update YQSF_DPII_JDRQ set jdzt = 222,pzr='" + p + "',pzyj='全部批准' where jdzt = 200";

            _jdrqDapperRepository.Execute(strSQL);
            _AppServices.CheckSjmxJdzt(id);
        }
        

        //正在检定
        public void SetWorking(int id)
        {
            string strSQL = "update YQSF_DPII_JDRQ set jdzt = 111 where id = " + id;
            _jdrqDapperRepository.Execute(strSQL);
        }

        public JDRQ Get(int id)
        {
            string strSQL = "select * from YQSF_DPII_JDRQ where id= " + id;
            IEnumerable<JDRQ> ret = _jdrqDapperRepository.Query(strSQL);
            if (ret.Count()>0)
                return ret.FirstOrDefault();
            else
                return null; // AddNew 
        }
        
    }
}
