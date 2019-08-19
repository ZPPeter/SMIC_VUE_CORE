using System.Collections.Generic;
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

namespace SMIC.SDIM
{
    public class SJMXAppServices : SMICAppServiceBase
    {
        private readonly IDapperRepository<SJMX, long> _sjmxDapperRepository;
        private readonly ICacheManager _cacheManager;//依赖注入缓存

        public SJMXAppServices(IDapperRepository<SJMX, long> sjmxDapperRepository, ICacheManager cacheManager)
        {
            _sjmxDapperRepository = sjmxDapperRepository;
            _cacheManager = cacheManager;//依赖注入缓存
        }

        public dynamic GetRecentSJMX()
        {
            string strSQL = @"SELECT TOP (10) a.ID, e.sjdid ,e.djrq ,d.QJMC , b.XHGGMC , a.ccbh ,a.jdzt
                            FROM dbo.YQSF_SJMX AS a INNER JOIN											
                            dbo.YQSF_SJD as e on a.sjdid = e.id INNER JOIN
                            dbo.JCXX_XHGG_BM AS b ON a.XHGGBM = b.XHGGBM LEFT JOIN
                            dbo.JCXX_QJMC_BM AS d ON b.QJMCBM = d.QJMCBM
                            ORDER BY a.ID DESC";
            IEnumerable<SJMX> ret = _sjmxDapperRepository.Query(strSQL);
            return ret;
        }

        public dynamic GetRecentSJMXBy(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return GetRecentSJMX();
            string strSQL = @"
SELECT top(10) * FROM (
SELECT * FROM (SELECT TOP (10) a.ID, e.sjdid ,e.djrq ,d.QJMC , b.XHGGMC , a.ccbh ,a.jdzt
                            FROM dbo.YQSF_SJMX AS a  RIGHT JOIN
														dbo.YQSF_SJD as e on a.sjdid = e.id LEFT JOIN
                            dbo.JCXX_XHGG_BM AS b ON a.XHGGBM = b.XHGGBM LEFT JOIN
                            dbo.JCXX_QJMC_BM AS d ON b.QJMCBM = d.QJMCBM                            
                            where a.ccbh like '%{0}%'
							order by a.id desc				
							) as a 
union all
SELECT * FROM (SELECT TOP (10) a.ID, e.sjdid ,e.djrq ,d.QJMC , b.XHGGMC , a.ccbh ,a.jdzt
                            FROM dbo.YQSF_SJMX AS a  RIGHT JOIN
														dbo.YQSF_SJD as e on a.sjdid = e.id LEFT JOIN
                            dbo.JCXX_XHGG_BM AS b ON a.XHGGBM = b.XHGGBM LEFT JOIN
                            dbo.JCXX_QJMC_BM AS d ON b.QJMCBM = d.QJMCBM                            
                            where e.sjdid like '%{0}%'
							order by a.id desc
														
) as b) as bb";

            strSQL = string.Format(strSQL, q.Trim());
            IEnumerable<SJMX> ret = _sjmxDapperRepository.Query(strSQL);
            return ret;
        }

    }
}