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

namespace SMIC.SDIM
{
    public class WTDAppServices : SMICAppServiceBase
    {
        private readonly IDapperRepository<WTD, long> _wtdDapperRepository;
        public WTDAppServices(IDapperRepository<WTD, long> wtdDapperRepository)
        {
            _wtdDapperRepository = wtdDapperRepository;
        }              
        
        public dynamic GetRecentWTD()
        {
            string strSQL = @"
            UPDATE YQSF_SJD
            SET yqjs = (
                SELECT COUNT(*) FROM YQSF_SJMX WHERE SJDID = YQSF_SJD.id
            )";
            _wtdDapperRepository.Execute(strSQL);

            strSQL = @"SELECT TOP(10) a.ID, a.SJDID , f.DWMC , a.SJRQ,YQJS
                              FROM dbo.YQSF_SJD AS a INNER JOIN 
                                   dbo.YQSF_KH AS f ON f.khid = a.khid
                              ORDER BY a.ID DESC";
            IEnumerable<WTD> ret = _wtdDapperRepository.Query(strSQL);
            return ret;
        }

        public dynamic GetRecentWTDBy(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return GetRecentWTD();
            string strSQL = @"select * from (select * from (SELECT TOP(10) a.ID, a.SJDID , f.DWMC , a.SJRQ,YQJS
                              FROM dbo.YQSF_SJD AS a INNER JOIN 
                                   dbo.YQSF_KH AS f ON f.khid = a.khid
                              where a.sjdid like '%{0}%'
                              ORDER BY a.ID DESC) as a
                              union
                              select * from (SELECT TOP(10) a.ID, a.SJDID , f.DWMC , a.SJRQ,YQJS
                              FROM dbo.YQSF_SJD AS a INNER JOIN 
                                   dbo.YQSF_KH AS f ON f.khid = a.khid
                              where f.dwmc like N'%{0}%'
                              ORDER BY a.ID DESC) as b) as c order by id desc";
            strSQL = string.Format(strSQL, q.Trim());
            IEnumerable<WTD> ret = _wtdDapperRepository.Query(strSQL);
            return ret;
        }

    }
}


