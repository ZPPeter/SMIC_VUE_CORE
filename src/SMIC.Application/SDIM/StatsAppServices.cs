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
    public class StatsAppServices : SMICAppServiceBase
    {
        private readonly IDapperRepository<STATS, long> _sjmxDapperRepository;
        private readonly ICacheManager _cacheManager;//依赖注入缓存

        public StatsAppServices(IDapperRepository<STATS, long> sjmxDapperRepository, ICacheManager cacheManager)
        {
            _sjmxDapperRepository = sjmxDapperRepository;
            _cacheManager = cacheManager;//依赖注入缓存
        }

        private double[] _getStatsData()
        {
            /*
            1000 全站仪             
            1030 GPS接收机
            1010 经纬仪
            1100 电子经纬仪
            1020 水准仪            
            1040 手持测距仪                  
            */
            double[] StatsData = new double[12];
            string strSQL = @"select 0 as y, 0 as m,count(0) as count ,b.qjmcbm as bm from YQSF_SJMX as a left join JCXX_XHGG_BM as b on a.xhggbm = b.XHGGBM GROUP BY b.QJMCBM ORDER BY b.QJMCBM ";

            IEnumerable<STATS> ret = _sjmxDapperRepository.Query(strSQL);
            StatsData[0] = ret.Where(c => c.bm == 1000).FirstOrDefault().count;
            StatsData[2] = ret.Where(c => c.bm == 1030).FirstOrDefault().count;
            StatsData[4] = ret.Where(c => (new int?[] { 1010, 1100 }).Contains(c.bm)).Sum(p => p.count);
            StatsData[6] = ret.Where(c => c.bm == 1020).FirstOrDefault().count;            
            StatsData[8] = ret.Where(c => c.bm == 1040).FirstOrDefault().count;
            StatsData[10] = ret.Where(c => !(new int?[] { 1000,1010,1100,1020,1030,1040 }).Contains(c.bm)).FirstOrDefault().count;

            strSQL = @"select 0 as y, 0 as m, count(0) as count,c.qjmcbm as bm from YQSF_SJMX as a 
            left join YQSF_SJD as b on a.sjdid = b.ID left join JCXX_XHGG_BM as c on a.xhggbm = c.XHGGBM 
            WHERE b.sjrq > dateadd(year, datediff(year, 0, getdate()), 0)
            GROUP BY c.QJMCBM ORDER BY c.QJMCBM";
            ret = _sjmxDapperRepository.Query(strSQL);
            StatsData[1] = ret.Where(c => c.bm == 1000).FirstOrDefault().count;
            StatsData[3] = ret.Where(c => c.bm == 1030).FirstOrDefault().count;            
            StatsData[5] = ret.Where(c => (new int?[] { 1010, 1100 }).Contains(c.bm)).Sum(p => p.count);
            StatsData[7] = ret.Where(c => c.bm == 1020).FirstOrDefault().count;            
            StatsData[9] = ret.Where(c => c.bm == 1040).FirstOrDefault().count;
            StatsData[11] = ret.Where(c => !(new int?[] { 1000, 1010, 1100, 1020, 1030, 1040 }).Contains(c.bm)).FirstOrDefault().count;


            double[] Datas = new double[] { StatsData[0], StatsData[1], StatsData[2], StatsData[3], StatsData[4], StatsData[5], StatsData[6], StatsData[7], StatsData[8], StatsData[9], StatsData[10], StatsData[11] };
            return Datas;
        }


        private double[][] _getStatsDataBy(NullableIdDto<int> input)
        {
            double[] StatsData = new double[12];
            string strSQL = @"select 
 Year(b.sjrq) as y,Month(b.sjrq) as m,c.QJMCBM as bm ,count(a.id) as count
from
    YQSF_SJMX as a left Join YQSF_SJD as b on a.sjdid=b.id
	  left join JCXX_XHGG_BM as c on a.xhggbm = c.XHGGBM
where b.sjrq>(SELECT DATEADD(yy, DATEDIFF(yy,0,getdate())-1, 0))
group by
    Year(b.sjrq),Month(b.sjrq),c.QJMCBM
		
order by
    Year(b.sjrq) desc, Month(b.sjrq) desc";

            IEnumerable<STATS> ret = _sjmxDapperRepository.Query(strSQL);
            IEnumerable<STATS> ret0 = null;
            int y = DateTime.Now.Year;
            double[] Datas1 = new double[]{ };
            double[] Datas2 = new double[]{ };
            if (input.Id != 9999)
            {
                for (int m = 1; m <= 12; m++)
                {
                    ret0 = ret.Where(c => (c.y == y && c.m == m && c.bm == input.Id));//.FirstOrDefault().count;
                    if (ret0.FirstOrDefault() != null)
                        StatsData[m - 1] = ret0.FirstOrDefault().count;
                }
                Datas1 = new double[] { StatsData[0], StatsData[1], StatsData[2], StatsData[3], StatsData[4], StatsData[5], StatsData[6], StatsData[7], StatsData[8], StatsData[9], StatsData[10], StatsData[11] };

                StatsData = new double[12];
                for (int m = 1; m <= 12; m++)
                {
                    ret0 = ret.Where(c => (c.y == y - 1 && c.m == m && c.bm == input.Id));//.FirstOrDefault().count;
                    if (ret0.FirstOrDefault() != null)
                        StatsData[m - 1] = ret0.FirstOrDefault().count;
                }
                Datas2 = new double[] { StatsData[0], StatsData[1], StatsData[2], StatsData[3], StatsData[4], StatsData[5], StatsData[6], StatsData[7], StatsData[8], StatsData[9], StatsData[10], StatsData[11] };
            }
            else {
                for (int m = 1; m <= 12; m++)
                {
                    ret0 = ret.Where(c => (c.y == y && c.m == m && !(new int?[] { 1000, 1030 }).Contains(c.bm)));
                    if (ret0.FirstOrDefault() != null)
                        StatsData[m - 1] = ret0.Sum(p => p.count);
                }
                Datas1 = new double[] { StatsData[0], StatsData[1], StatsData[2], StatsData[3], StatsData[4], StatsData[5], StatsData[6], StatsData[7], StatsData[8], StatsData[9], StatsData[10], StatsData[11] };

                StatsData = new double[12];
                for (int m = 1; m <= 12; m++)
                {
                    ret0 = ret.Where(c => (c.y == y - 1 && c.m == m && !(new int?[] { 1000, 1030 }).Contains(c.bm)));
                    if (ret0.FirstOrDefault() != null)
                        StatsData[m - 1] = ret0.Sum(p => p.count);
                }
                Datas2 = new double[] { StatsData[0], StatsData[1], StatsData[2], StatsData[3], StatsData[4], StatsData[5], StatsData[6], StatsData[7], StatsData[8], StatsData[9], StatsData[10], StatsData[11] };
            }

            double[][] Datas = new double[][] {Datas1, Datas2};

            return Datas;

        }


        /// <summary>
        /// 缓存
        /// </summary>
        /// <returns></returns>
        public double[] getStatsData()
        {
            var entityCache = _cacheManager.GetCache("StatsCache").Get("AllDatas", () => _getStatsData());
            return entityCache;
        }

        public double[][] getStatsDataBy(NullableIdDto<int> input)
        {
            var entityCache = _cacheManager.GetCache("StatsCacheBy").Get("DatasBy"+ input.Id, () => _getStatsDataBy(input));
            return entityCache;
        }

    }
}
