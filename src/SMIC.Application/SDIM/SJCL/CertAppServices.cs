using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using SMIC.PhoneBooks.Persons;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Abp.Specifications;
using System.Reflection;
using SMIC.SJCL;
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using SMIC.SJCL.Common;
using System.Text;

using System.Diagnostics;
using System.Data;
using System.Linq;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Abp.Dapper.Filters;

using Abp.Timing; // Clock.Now;
using Newtonsoft.Json;
using DapperExtensions;
using Abp.Data;
using Abp.Runtime.Caching;
using SMIC.SDIM.SJCL.Dtos;
using Microsoft.AspNetCore.Identity;
using SMIC.Authorization.Users;
namespace SMIC.SDIM.SJCL
{
    public class CertAppServices : SMICAppServiceBase
    {
        private readonly IDapperRepository<SJMX_ZSBH> _sjmxDapperRepository;
        private readonly IDapperRepository<ZSH_DATA> _zshDapperRepository;
        private readonly IMemoryCache _cache;
        private readonly PluginsOptions _options;
        private readonly UserManager _userManager;
        //IRepository<User, long> repository,

        public CertAppServices(
            IDapperRepository<SJMX_ZSBH> sjmxDapperRepository,
            IDapperRepository<ZSH_DATA> zshDapperRepository,
            IMemoryCache cache,
            IOptions<PluginsOptions> optionsAccessor,
            UserManager userManager
            )
        {
            _sjmxDapperRepository = sjmxDapperRepository;
            _zshDapperRepository = zshDapperRepository;
            _cache = cache;
            _options = optionsAccessor.Value;
            _userManager = userManager;
        }

        /// <summary>
        /// 取证书编号
        /// LB
        /// M01 GPS
        /// M02 全站仪
        /// </summary>
        /// <param name="LB"></param>
        /// <returns></returns>
        private string GetZSBH(string LB, double ID)
        //public dynamic GetZSBH(string LB, double ID)
        {
            StringBuilder strSQL = new StringBuilder(100);
            strSQL.Append("declare @SQL VARCHAR(255)");
            strSQL.Append("\r\n");
            strSQL.Append("DECLARE @Year decimal");
            strSQL.Append("\r\n");
            strSQL.Append("DECLARE @Zsbh VARCHAR(12)");
            strSQL.Append("\r\n");
            strSQL.Append("set @Year = (select LEFT(Right(MAX(zsbh), 8), 4) from YQSF_SJMX where zsbh like '" + LB + "-%')");
            strSQL.Append("\r\n");
            strSQL.Append("if (year(getdate()) > @Year) ");
            strSQL.Append("\r\n");
            strSQL.Append("set @Zsbh = '" + LB + "-" + DateTime.Now.Year + "0001'");
            strSQL.Append("\r\n");
            strSQL.Append("else");
            strSQL.Append("\r\n");
            strSQL.Append("set @Zsbh = '" + LB + "-'+str((select Right(MAX(zsbh),8)+1 from YQSF_SJMX where zsbh like '" + LB + "-%'),8,0)");
            strSQL.Append("\r\n");
            strSQL.Append("update yqsf_sjmx set zsbh=@Zsbh where ID=" + ID + "  and len(zsbh)=0");
            strSQL.Append("\r\n");
            strSQL.Append("select zsbh from YQSF_SJMX where ID=" + ID);
            return _sjmxDapperRepository.Query(strSQL.ToString()).FirstOrDefault().ZSBH;
        }

        private void AddtoZshData(int ID, string Data)
        {
            long jdyid = (long)AbpSession.UserId;
            //jdzt = 111
            //string userName = AbpSession.GetUserName();//获取当前登录用户 surName
            var param = new
            {
                ID = ID,
                Data = Data,
                JDRQ = DateTime.Now,
                JDYID = jdyid
            };
            string strSQL = @"delete from ZSH_Data where ID=@ID 
                              insert into ZSH_Data values(@ID,@Data)
                              update SJCL_CHYQ set JDRQ = @JDRQ,JDZT = 111,JDYID = @JDYID where ID=@ID";
            int ret = _sjmxDapperRepository.Execute(strSQL, param);
            // ToDo 主页数据刷新
        }

        /// <summary>
        /// 生成证书
        /// </summary>
        /// <returns></returns>
        public async Task<string[]> MakeCert(CertDto ipt)
        //public async Task<string[]> MakeCert(JDJLFM jdjlfm, RawTemplate rawTemplate, int[] Signer)        
        {
            if (string.IsNullOrWhiteSpace(ipt.rawTemplate.MBMC))
            {
                return null;
            }
            var plugin = await PluginFactory.GetPlugin(_cache, _options, ipt.rawTemplate.MBMC);
            if (plugin != null)
            {
                var zsbh = GetZSBH(ipt.rawTemplate.MBMC, ipt.jdjlfm.ID);
                ipt.jdjlfm.ZSBH = zsbh;
                string[] ret = plugin.Handle(ipt.rawTemplate, ipt.jdjlfm, ipt.Signer);
                string resData = JsonConvert.SerializeObject(ret);
                AddtoZshData(ipt.jdjlfm.ID, resData);
                return ret;
            }
            return null;
        }

        public string ShowResults(int ID)
        {
            string strSQL = @"select Data from ZSH_Data where ID=@ID";
            var param = new
            {
                ID = ID
            };
            return _zshDapperRepository.Query(strSQL, param).FirstOrDefault().Data;
        }

        public int SetJDWB(int ID)
        {
            string strSQL = @"update SJCL_CHYQ set JWRQ = @JWRQ,JDZT = 122 where ID=@ID";
            var param = new
            {
                ID = ID,
                JWRQ = DateTime.Now
            };
            return _zshDapperRepository.Execute(strSQL, param);
        }

        public async Task<int> SetHYWB(int ID)
        {
            long jdyid = (long)AbpSession.UserId;
            var user = await _userManager.GetUserByIdAsync(jdyid);

            string strSQL = @"update SJCL_CHYQ set HYYJ = @HYYJ,HYYID=@HYYID,HYY=@HYY,JDZT = 200 where ID=@ID";
            var param = new
            {
                ID = ID,
                HYYID = jdyid,
                HYY = user.Surname,
                HYYJ = DateTime.Now.ToString()
            };
            return _zshDapperRepository.Execute(strSQL, param);
        }

        public async Task<int> SetPZWB(int ID)
        {
            long jdyid = (long)AbpSession.UserId;
            var user = await _userManager.GetUserByIdAsync(jdyid);

            string strSQL = @"update SJCL_CHYQ set PZRID=@PZRID,PZR=@PZR,PZYJ=@PZYJ,JDZT = 222 where ID=@ID";
            var param = new
            {
                ID = ID,
                PZRID = jdyid,
                PZR = user.Surname,
                PZYJ = DateTime.Now.ToString()
            };
            return _zshDapperRepository.Execute(strSQL, param);
        }

        public async Task<int> SetQBWB()
        {
            long jdyid = (long)AbpSession.UserId;
            var user = await _userManager.GetUserByIdAsync(jdyid);
            
            string strSQL = @"update SJCL_CHYQ set PZRID=@PZRID,PZR=@PZR,PZYJ='ALL:'+@PZYJ,JDZT = 222 where JDZT=200";
            var param = new
            {
                PZRID = jdyid,
                PZR = user.Surname,
                PZYJ = DateTime.Now.ToString()
            };
            return _zshDapperRepository.Execute(strSQL, param);
        }
    }
}
