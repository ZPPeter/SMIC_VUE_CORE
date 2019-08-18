using System.Diagnostics;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using SMIC.Members.Dto;

using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Abp.Dapper.Repositories;

using Abp.Dapper.Filters;

using Abp.Timing; // Clock.Now;
using Newtonsoft.Json;
using SMIC.Roles;
using SMIC.Authorization.Users;
using SMIC.Authorization.Roles;
using System.Linq.Expressions;
using System.Text;
using DapperExtensions;
using Abp.Data;
using Abp.Runtime.Caching;
namespace SMIC.Members
{
    /// <summary>
    /// 仅供后台查询会员信息
    /// </summary>
    public class MemberUserCacheAppService : AsyncCrudAppService<MemberUser, MemberUserDto, long, PagedMemberUserResultRequestDto, CreateMemberUserDto, MemberUserDto>, IMemberUserAppService
    {
        private readonly ICacheManager _cacheManager;//依赖注入缓存
        private readonly IDapperRepository<AbpUser, long> _userRepository;
    
        public MemberUserCacheAppService(
            IRepository<MemberUser, long> repository, IDapperRepository<AbpUser, long> userRepository, ICacheManager cacheManager)
            : base(repository)
        {
            _userRepository = userRepository;
            _cacheManager = cacheManager;//依赖注入缓存
        }


        public IEnumerable<AbpUser> GetAll()
        {
            //IEnumerable<AbpUser> ret = _userRepository.GetAll();
            //return ret;

            var entityCache = _cacheManager.GetCache("MemberUserCache")
                              .Get("MemberUser", () => _userRepository.GetAll());

            // ITypedCache泛型版本
            //var entityCache2 = _cacheManager.GetCache("ControllerCache")
            //                      .AsTyped<string, List<AbpUser>>()
            //                      .Get("AllUsers", () => _userRepository.GetAll());

            return entityCache;
        }
        

    }
}