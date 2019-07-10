﻿using System;
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
using System.Text; // String 字符串一旦创建就不可修改大小,在需要对字符串执行重复修改的情况下，与创建新的String对象相关的系统开销可能会非常昂贵。如果要修改字符串而不创建新的对象，则可以使用System.Text.StringBuilder类。例如当在一个循环中将许多字符串连接在一起时，使用StringBuilder类可以提升性能。
namespace SMIC.Members
{
    /// <summary>
    /// 仅供后台查询会员信息
    /// </summary>
    public class MemberUserAppService : AsyncCrudAppService<MemberUser, MemberUserDto, long, PagedMemberUserResultRequestDto, CreateMemberUserDto, MemberUserDto>, IMemberUserAppService
    {
        //private readonly IDapperRepository<MemberUser, long> _memberDapperRepository;
        private readonly IDapperRepository<AbpUser, long> _userRepository;
        private readonly IDapperRepository<Role, int> _roleRepository;

        public MemberUserAppService(
            IRepository<MemberUser, long> repository, IDapperRepository<AbpUser, long> userRepository, IDapperRepository<Role, int> roleRepository)//, IDapperRepository<MemberUser, long> memberDapperRepository)
            : base(repository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;

            //_memberDapperRepository = memberDapperRepository;            
            LocalizationSourceName = SMICConsts.LocalizationSourceName;
        }

        protected override IQueryable<MemberUser> CreateFilteredQuery(PagedMemberUserResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), b => b.Name.Contains(input.Keyword))
                .WhereIf(input.From.HasValue, b => b.CreationTime >= input.From.Value.LocalDateTime)
                .WhereIf(input.To.HasValue, b => b.CreationTime <= input.To.Value.LocalDateTime);
            //CreationTime是按照服务器时间存储，故表单提交的UTC时间可转为服务器LocalDateTime进行比较
        }

        public PagedResultDto<AbpUser> GetPagedMemberUsers(PagedMemberUserResultRequestDto input)
        {

            //Expression<Func<AbpUser,Role, bool>> predicat = (p,b) => (p.TenantId == null &&  p.RoleNames );

            /*
             _repository -> MemberUser -> 查询记录 from AbpUsers where UserType =1
                            user -> UserType =0 也会显示，TenantId = 1 不显示，未启用租户,User 没有 LastLoginTime 的
             */

            Expression<Func<AbpUser, bool>> predicate = p => p.TenantId == null;
                        
            var totalCount = _userRepository.Count(predicate);
            IEnumerable<AbpUser> ret = _userRepository.GetAllPaged(
                predicate,
                input.SkipCount / input.MaxResultCount,
                input.MaxResultCount,
                input.Sorting, input.Order == "asc"); // input.Order=="asc"  true/false

            //List<AbpUser> tempList2 = ret.MapTo<List<AbpUser>>();
            List<AbpUser> tempList = new List<AbpUser>();
            IEnumerator<AbpUser> currentEnumerator = ret.GetEnumerator();
            if (currentEnumerator != null)
            {
                for (int count = 0; currentEnumerator.MoveNext(); count++)
                {
                    //currentEnumerator.Current.RoleNames = GetRoles(currentEnumerator.Current.Id);
                    tempList.Add(currentEnumerator.Current);
                }
            }

            return new PagedResultDto<AbpUser>(
                totalCount,
                tempList
            );
        }

        public string[] GetRoles(long userid) {
                        
            var param = new { Id = userid };
            var ret = _roleRepository.Query("select b.NormalizedName from AbpRoles b where b.Id in (select RoleId from AbpUserRoles a where a.UserId = @Id)", param);

            List<string> list = new List<string>();
            foreach (Role r in ret)
                list.Add(r.NormalizedName);            
            return list.ToArray();
        }

        /*
         * DapperRepository.Query - 需要写分页 SQL
        public PagedResultDto<MemberUser> GetMemberUsers(PagedMemberUserResultRequestDto input)
        {
            StringBuilder strSql = new StringBuilder("select Id,Name,userName,isActive,CreationTime,LastLoginTime from AbpUsers where TenantId is null");
            
            dynamic ret = _memberDapperRepository.Query(strSql.ToString());

            List<MemberUser> tempList = new List<MemberUser>();
            IEnumerator<MemberUser> currentEnumerator = ret.GetEnumerator();
            if (currentEnumerator != null)
            {
                for (int count = 0; currentEnumerator.MoveNext(); count++)
                {
                    tempList.Add(currentEnumerator.Current);
                }
            }
            return new PagedResultDto<MemberUser>(
                tempList.Count,
                tempList
            );
        }

        //public IEnumerable<MemberUser> GetDapperMemberUsers()
        //public string GetDapperPersons()
        //public IEnumerable<Person> GetDapperPersons()
        //{
            //SqlParameter[] parameters = new[]{
            //    new SqlParameter("Id", AbpSession.UserId ),
            //    new SqlParameter("LastLoginTime", Clock.Now)
            //};
            //_personDapperRepository.Execute("update AbpUsers set LastLoginTime2=@LastLoginTime where Id=@Id", parameters); // x

            //Logger.Info("update AbpUsers set LastLoginTime = '" + Clock.Now + "' where Id = " + AbpSession.UserId);

            //string sql = "update AbpUsers set LastLoginTime2 = @LastLoginTime WHERE Id = @Id;";
            //var singleParam = new { Id = AbpSession.UserId, LastLoginTime = Clock.Now };
            //_personDapperRepository.Execute(sql, singleParam);

            //_personDapperRepository.Execute("update AbpUsers set LastLoginTime = '" + Clock.Now + "' where Id = " + AbpSession.UserId);

            //Logger.Info("Update "+ AbpSession.UserId + "OK? Than show it.");

            //IEnumerable<Person> persons = _personDapperRepository.Query("select * from Persons");
            //return persons;
            //var lst = _personDapperRepository.Query("select LastLoginTime2 from AbpUsers");

            //var lst = _personDapperRepository.Query("select Id,Name,userName,isActive,CreationTime,LastLoginTime2 from AbpUsers");
            //return JsonConvert.SerializeObject(lst);

            //IEnumerable<MyUser> persons = _personDapperRepository.Query("select Id,Name,userName,isActive,CreationTime,LastLoginTime2 LastLoginTime from AbpUsers");
            //return persons;

            //await _DapperRepository.QueryAsync("select * from table");
            //await _DapperRepository.CounAsync(t => t.SysConfigName != "");

            //IEnumerable<MemberUser> persons = _memberDapperRepository.Query("select Id,Name,userName,isActive,CreationTime,LastLoginTime2 LastLoginTime from AbpUsers");
            //IEnumerable<MemberUser> rets = _memberDapperRepository.Query("select Id,Name,userName,isActive,CreationTime,LastLoginTime from AbpUsers");
            //return rets;
        //}
        */

        /// <summary>
        /// 仅供查询使用，调用Create/Update/Delete会返回异常
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public override Task<MemberUserDto> Create(CreateMemberUserDto input) => throw new NotImplementedException();

        /// <summary>
        /// 仅供查询使用，调用Create/Update/Delete会返回异常
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public override Task<MemberUserDto> Update(MemberUserDto input) => throw new NotImplementedException();

        /// <summary>
        /// 仅供查询使用，调用Create/Update/Delete会返回异常
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RemoteService(false)]
        public override Task Delete(EntityDto<long> input) => throw new NotImplementedException();
    }
}
