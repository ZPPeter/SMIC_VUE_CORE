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
using System.Text;
using Abp.Domain.Uow;

namespace SMIC.SDIM
{
    public class SJMXAppServices : SMICAppServiceBase
    {
        private readonly IDapperRepository<JDRQ, long> _jdrqDapperRepository;
        private readonly IDapperRepository<SJMX, long> _sjmxDapperRepository;
        private readonly IDapperRepository<STATS, long> _DapperRepository;
        private readonly ICacheManager _cacheManager;//依赖注入缓存

        private readonly JDRQAppServices _jdrqAppServices;//依赖注入缓存


        public SJMXAppServices(IDapperRepository<SJMX, long> sjmxDapperRepository, IDapperRepository<JDRQ, long> jdrqDapperRepository, ICacheManager cacheManager,IDapperRepository<STATS, long> DapperRepository, JDRQAppServices jdrqAppServices)  
        {
            _jdrqDapperRepository = jdrqDapperRepository;
            _sjmxDapperRepository = sjmxDapperRepository;
            _cacheManager = cacheManager;//依赖注入缓存
            _DapperRepository = DapperRepository;
            _jdrqAppServices = jdrqAppServices;            
        }

        public SJMX Get(int id)
        {
            string strSQL = @"SELECT a.ID, e.sjdid ,e.djrq ,d.QJMC , b.XHGGMC , a.ccbh ,a.jdzt
                            FROM dbo.YQSF_SJMX AS a INNER JOIN											
                            dbo.YQSF_SJD as e on a.sjdid = e.id INNER JOIN
                            dbo.JCXX_XHGG_BM AS b ON a.XHGGBM = b.XHGGBM LEFT JOIN
                            dbo.JCXX_QJMC_BM AS d ON b.QJMCBM = d.QJMCBM
                            where a.ID={0}";
            strSQL = string.Format(strSQL, id);
            IEnumerable<SJMX> ret = _sjmxDapperRepository.Query(strSQL);
            if (ret != null)
                return ret.FirstOrDefault();
            else
                return null;
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
        
        //委托单下面的List Sjmx
        public IEnumerable<SJMX> GetSjmxs(string sjdid)
        {
            string strSQL = @"
SELECT a.sjdid,qjmc,djrq,xhggmc,b.xhggbm,ccbh,zzcnr as zzc,jdy,a.bzsm,g.jdrq,g.jwrq,f.dwmc as wtdw,DATEADD(day,14,djrq) as yqjcrq,a.jdzt as jdzt1,g.jdzt as jdzt2
FROM dbo.YQSF_SJMX AS a LEFT JOIN
dbo.YQSF_SJD as e on a.sjdid = e.id LEFT JOIN
dbo.YQSF_KH as f on e.khid = f.khid LEFT JOIN
dbo.JCXX_XHGG_BM AS b ON a.XHGGBM = b.XHGGBM LEFT JOIN
dbo.JCXX_ZZC_BM AS c ON b.ZZCBM = c.ZZCBM LEFT JOIN
dbo.JCXX_QJMC_BM AS d ON b.QJMCBM = d.QJMCBM LEFT JOIN
dbo.YQSF_DPII_JDRQ as g on g.id = a.id 											
where d.QJMCBM = 1000 and e.djrq>'2019-04-21' and a.sjdid ='{0}'
";
            strSQL = string.Format(strSQL, sjdid);
            IEnumerable<SJMX> ret = _sjmxDapperRepository.Query(strSQL);

            /* 前端处理            
                    string jdzt = reader["jdzt"].ToString();
                    string jdzt2 = reader["检定状态"].ToString();
                    if (jdzt2 == "检完")
                    {
                        sjmx.jdzt = 222;
                    }
                    else if (jdzt != "")
                        sjmx.jdzt = Int32.Parse(jdzt);
                    sjmx.img = GetZzcImg(zzc);
                    sjmx.jbcs = jbcs.GetJbcs(reader["xhggbm"].ToString());
           */
                    
            return ret;
        }

        
        //Search
        public IEnumerable<SJMX> ListSjmxs(string lb, string q, string userId)
        {

            string strSQL = @"select top 20 a.sjdid,qjmc,djrq,xhggmc,b.xhggbm,ccbh,zzcnr as zzc,jdy,a.bzsm,g.jdrq,g.jwrq,f.dwmc as wtdw,DATEADD(day, 14, djrq) as yqjcrq,a.jdzt as jdzt1,g.jdzt as jdzt2
        FROM dbo.YQSF_SJMX AS a LEFT JOIN
        dbo.YQSF_SJD as e on a.sjdid = e.id LEFT JOIN
        dbo.YQSF_KH as f on e.khid = f.khid LEFT JOIN
        dbo.JCXX_XHGG_BM AS b ON a.XHGGBM = b.XHGGBM LEFT JOIN
        dbo.JCXX_ZZC_BM AS c ON b.ZZCBM = c.ZZCBM LEFT JOIN
        dbo.JCXX_QJMC_BM AS d ON b.QJMCBM = d.QJMCBM LEFT JOIN
        dbo.YQSF_DPII_JDRQ as g on g.id = a.id
        where d.QJMCBM = 1000 and e.djrq > '2019-04-21'";                             // 查询searchAll
                        
            if (lb == "2")
                strSQL += " and  g.jdzt is null and a.jdzt<>'检完'";                  // 待检:JDZT is null
            else if (lb == "3")
                strSQL += " and  g.jdzt=122 and a.jdzt<>'检完' and jdy<>" + userId;   // 待核验
            else if (lb == "4")
                strSQL += " and  g.jdzt=200 and a.jdzt<>'检完' ";                     // 待批准
            else if (lb == "9")
                strSQL += " and (g.jdzt<122 or g.jdzt IS NULL) and a.jdzt <>'检完' "; // 包含在检

            if (q != "")
            {
                strSQL += " and (出厂编号 like '%" + q + "%' or 型号规格 like '%" + q + "%'";
                if (lb == "9")
                {
                    strSQL += " or 委托单位 like '%" + q + "%'";
                    strSQL += " or 委托单号码 like '%" + q + "%'";
                }
                strSQL += ")";
            }
            if (lb == "9")
                strSQL += " ORDER BY ID DESC";

            IEnumerable<SJMX> ret = _sjmxDapperRepository.Query(strSQL);
            return ret;
        }

        //Search By WTDH
        public IEnumerable<SJMX> GetSjmxByWtdh(string q) // ???
        {
            string strSQL = @"
select top 20 a.sjdid,e.sjdid as wtdh,qjmc,djrq,xhggmc,b.xhggbm,ccbh,zzcnr as zzc,jdy,a.bzsm,g.jdrq,g.jwrq,f.dwmc as wtdw,DATEADD(day, 14, djrq) as yqjcrq,a.jdzt as jdzt1,g.jdzt as jdzt2
        FROM dbo.YQSF_SJMX AS a LEFT JOIN
        dbo.YQSF_SJD as e on a.sjdid = e.id LEFT JOIN
        dbo.YQSF_KH as f on e.khid = f.khid LEFT JOIN
        dbo.JCXX_XHGG_BM AS b ON a.XHGGBM = b.XHGGBM LEFT JOIN
        dbo.JCXX_ZZC_BM AS c ON b.ZZCBM = c.ZZCBM LEFT JOIN
        dbo.JCXX_QJMC_BM AS d ON b.QJMCBM = d.QJMCBM LEFT JOIN
        dbo.YQSF_DPII_JDRQ as g on g.id = a.id
        where d.QJMCBM = 1000 and e.djrq > '2019-04-21' and e.sjdid='" + q + "'";

            IEnumerable<SJMX> ret = _sjmxDapperRepository.Query(strSQL);
            return ret;
        }
    
        // public static List<int> GetMonthCount(int year) -> StatsAppServices.cs

        public void CheckSetOver()
        {
            string strSQL = "Update YQSF_SJD set qzyjdzt='检定完毕' where qzyjs>0 and qzyjdzt<>'检定完毕' and qzyjs=(select count(*) from YQSF_SJMX where jdzt='检完' and SJDID=YQSF_SJD.id)";
            _sjmxDapperRepository.Execute(strSQL);
            strSQL = @"
            Update YQSF_SJD set qzyjdzt='检定完毕' where qzyjs>0 and qzyjdzt<>'检定完毕' and qzyjs=(
            select count(0)
            FROM dbo.YQSF_SJMX AS a LEFT JOIN
            dbo.YQSF_SJD as e on a.sjdid = e.id LEFT JOIN
            dbo.YQSF_DPII_JDRQ as g on g.id = a.id
            where (g.jdzt='222' or a.jdzt='检完') and a.sjdid=a.id)";
            _sjmxDapperRepository.Execute(strSQL);
        }

        //重新检定
        public void ResetJdzt(string id)
        {
            string strSQL = "update YQSF_SJD set qzyjdzt = '正在检定',jdzt='登记' where ID = (select sjdid from YQSF_SJMX as b where b.id = " + id + ")";
            _sjmxDapperRepository.Execute(strSQL);
            strSQL = "Update YQSF_SJMX set jdzt='登记' where id=" + id;
            _sjmxDapperRepository.Execute(strSQL);
            strSQL = "Update YQSF_DPII_JDRQ set jdzt=100 where id=" + id;
            _sjmxDapperRepository.Execute(strSQL);
        }

        public int UpdateCcbh(int id, string ccbh)
        {
            string strSQL = "update YQSF_SJMX set ccbh = '" + ccbh + "' where id = " + id;
            return _sjmxDapperRepository.Execute(strSQL);
        }


        /*
         * 构造函数注入 JDRQAppService 或者在 Domain Service (Core) 处理
        public JDRQ GetJdrq(int id)
        {
            string strSQL = "select * from YQSF_DPII_JDRQ where id= " + id;
            IEnumerable<JDRQ> ret = _jdrqDapperRepository.Query(strSQL);
            if (ret != null)
                return ret.FirstOrDefault();
            else
                return null; // AddNew 
        }
        */


        public string GetSjmxJdzt(int id) {
            string jdzt0 = "";
            int? jdzt1 = null;
            SJMX sjmx = Get(id);
            if (sjmx != null)
                jdzt0 = sjmx.jdzt;
                        
            JDRQ jdrq = _jdrqAppServices.Get(id);
            //JDRQ jdrq = GetJdrq(id);
            if (jdrq != null)
                jdzt1 = jdrq.jdzt;

            if (jdzt1 != null)
                jdzt0 += jdzt1.ToString();

            if (jdzt0.IndexOf("222") > 0)
            {
                _jdrqAppServices.SetOverByJdzt(id, 222);
            }

            return jdzt0;            
        }

        public void CheckSjmxJdzt(int id)
        {
            string strSQL = "select 0 as y, 0 as m, qzyjs as count, id as bm from YQSF_SJD where ID = (select sjdid from YQSF_SJMX as b where b.id = {0}";
            STATS st = _DapperRepository.Query(string.Format(strSQL, id)).FirstOrDefault();
            int yqjs = st.count;
            int wtdid = st.bm;
            strSQL = @"SELECT 0 as y, 0 as m, qzyjs as count, 0 as bm
        FROM dbo.YQSF_SJMX AS a LEFT JOIN
        dbo.YQSF_SJD as e on a.sjdid = e.id LEFT JOIN
        dbo.JCXX_XHGG_BM AS b ON a.XHGGBM = b.XHGGBM LEFT JOIN
        dbo.JCXX_QJMC_BM AS d ON b.QJMCBM = d.QJMCBM								
        where d.QJMCBM = 1000 and e.djrq>'2019-04-21' and jdzt=222 and e.sjdid={0}";
            st = _DapperRepository.Query(string.Format(strSQL, wtdid)).FirstOrDefault();
            int yqjs1 = st.count;

            strSQL = "select 0 as y, 0 as m, count(0) as count,0 as bm from YQSF_SJMX where jdzt = '检完' and ID = {0}";
            st = _DapperRepository.Query(string.Format(strSQL, id)).FirstOrDefault();
            int yqjs2 = st.count;

            if (yqjs == yqjs1 || yqjs == yqjs2) {
                strSQL = "update YQSF_SJD set qzyjdzt = N'检定完毕' where id = {0}";                
                st = _DapperRepository.Query(string.Format(strSQL, wtdid)).FirstOrDefault();
            }
        }

        //img:  0leica 1topcon 2trimble 3sokkia 4south 5nikkom
        public static string GetZzcImg(string zzc)
        {
            string img = "0";
            if (zzc.Contains("徕卡"))
                img = "1";
            else if (zzc.Contains("拓普康"))
                img = "2";
            else if (zzc.Contains("天宝"))
                img = "3";
            else if (zzc.Contains("索佳"))
                img = "4";
            else if (zzc.Contains("南方"))
                img = "5";
            else if (zzc.Contains("尼康"))
                img = "6";
            return img;
        }
    }
}