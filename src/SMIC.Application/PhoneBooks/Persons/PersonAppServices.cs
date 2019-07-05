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
using SMIC.PhoneBooks.Persons.Authorization;
using SMIC.PhoneBooks.Persons.DomainServices;
using SMIC.PhoneBooks.Persons.Dtos;
using SMIC.PhoneBooks.PhoneNumbers;
using SMIC.PhoneBooks.PhoneNumbers.Dtos;
using Abp.Dapper.Repositories;
//using System.Data.SqlClient;
using Abp.Timing; // Clock.Now;
using Newtonsoft.Json;

using SMIC.Members;
using System.Linq.Expressions;

using System.Linq;
using System;

namespace SMIC.PhoneBooks.Persons
{
    /// <summary>
    ///     Person应用层服务的接口实现方法
    /// </summary>
    [AbpAuthorize(PersonAppPermissions.Person)]
    public class PersonAppService : SMICAppServiceBase, IPersonAppService
    {
        private readonly IPersonManager _personManager;

        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        ///
        private readonly IDapperRepository<Person> _personDapperRepository;
        private readonly IDapperRepository<MemberUser, long> _memberDapperRepository;
        private readonly IDapperRepository<VW_SJMX, long> _vwsjmxDapperRepository;

        private readonly IRepository<Person, int> _personRepository;
        private readonly IRepository<PhoneNumber, long> _phoneNumbeRepository;


        /// <summary>
        ///     构造函数
        /// </summary>
        public PersonAppService(
            IRepository<Person, int> personRepository,
            IPersonManager personManager,
            IRepository<PhoneNumber, long> phoneNumbeRepository,
            IDapperRepository<Person> personDapperRepository,
            IDapperRepository<MemberUser, long> memberDapperRepository,
            IDapperRepository<VW_SJMX, long> vwsjmxDapperRepository)
        {
            _personRepository = personRepository;
            _personManager = personManager;
            _phoneNumbeRepository = phoneNumbeRepository;
            _personDapperRepository = personDapperRepository;
            _memberDapperRepository = memberDapperRepository;
            _vwsjmxDapperRepository = vwsjmxDapperRepository;
        }

        //public string GetDapperPersons()
        //public IEnumerable<Person> GetDapperPersons()
        public IEnumerable<MemberUser> GetDapperPersons()
        {
            //SqlParameter[] parameters = new[]{
            //    new SqlParameter("Id", AbpSession.UserId ),
            //    new SqlParameter("LastLoginTime", Clock.Now)
            //};
            //_personDapperRepository.Execute("update AbpUsers set LastLoginTime2=@LastLoginTime where Id=@Id", parameters); // x

            Logger.Info("update AbpUsers set LastLoginTime2 = '" + Clock.Now + "' where Id = " + AbpSession.UserId);

            //string sql = "update AbpUsers set LastLoginTime2 = @LastLoginTime WHERE Id = @Id;";
            //var singleParam = new { Id = AbpSession.UserId, LastLoginTime = Clock.Now };
            //_personDapperRepository.Execute(sql, singleParam);

            _personDapperRepository.Execute("update AbpUsers set LastLoginTime2 = '" + Clock.Now + "' where Id = " + AbpSession.UserId);

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
            IEnumerable<MemberUser> persons = _memberDapperRepository.Query("select Id,Name,userName,isActive,CreationTime,LastLoginTime2 from AbpUsers");
            return persons;

        }

        //public IEnumerable<VW_SJMX> GetSjmx()
        //public List<VW_SJMX> GetSjmx()
        public dynamic GetSjmx1()
        {
            dynamic ret = _vwsjmxDapperRepository.GetAllPaged(x => x.器具名称 == "全站仪", 1, 20, "ID").ToDynamicList<dynamic>(); //OK
            //IEnumerable<VW_SJMX> ret = _vwsjmxDapperRepository.Query("select top 20 * from VW_SJMX"); //OK                        
            return ret;
        }


        public int GetPersonCount()
        {
            Expression<Func<Person, bool>> predicate = p=>p.Name != "";            
            predicate = predicate.And(p => p.Name.Contains("A"));
            return _personDapperRepository.Count(predicate);
        }

        public PagedResultDto<VW_SJMX> GetSjmx2(GetVwSjmxsInput input)
        {

            Expression condition = null;
            ParameterExpression param = Expression.Parameter(typeof(VW_SJMX), "p");
            Expression right = Expression.Constant("-1");
            Expression left = Expression.Property(param, typeof(VW_SJMX).GetProperty("送检单号"));
            Expression filter = Expression.NotEqual(left, right); // 送检单号 != -1
            condition = filter;

            right = Expression.Constant("12345");
            left = Expression.Property(param, typeof(VW_SJMX).GetProperty("送检单号"));
            filter = Expression.Equal(left, right);
            condition = Expression.And(condition, filter);

            //var predicate = PredicateExtensions.True<VW_SJMX>();
            var predicate = Expression.Lambda<Func<VW_SJMX, bool>>(condition, param);
            //predicate = predicate.And(p => p.送检单号.Contains(input.Filter));

            /*
            if (!input.Filter.IsNullOrWhiteSpace())
            {
                predicate = predicate.And(p => p.送检单号.Contains(input.Filter));
                predicate = predicate.Or(p => p.单位名称.Contains(input.Filter));
            }
            if (input.From != null) // > DateTime.MinValue
            {
                predicate = predicate.And(p => p.送检日期 >= input.From);
            }
            if (input.To != null) // > DateTime.MinValue
            {
                predicate = predicate.And(p => p.送检日期 <= input.To);
            }
            */

            var totalCount = _vwsjmxDapperRepository.Count(predicate);

            /*
            var totalCount1 = _vwsjmxDapperRepository.Count(
                a => (a.器具名称 == "全站仪") &&
                a.送检单号.Contains(input.Filter) &&
                a.送检日期 >= input.From &&
                a.送检日期 <= input.To
                );            
            */
            //IEnumerable<VW_SJMX> ret = _vwsjmxDapperRepository.GetAllPaged(x => x.器具名称 == "全站仪", 2, 20, "ID");
            //IEnumerable<VW_SJMX> ret = _vwsjmxDapperRepository.GetAllPaged(a =>(a.器具名称 == "全站仪") && a.送检单号.Contains(input.Filter), input.SkipCount/input.MaxResultCount, input.MaxResultCount, input.Sorting);

            IEnumerable<VW_SJMX> ret = _vwsjmxDapperRepository.GetAllPaged(
                predicate,
                input.SkipCount / input.MaxResultCount,
                input.MaxResultCount,
                input.Sorting);


            List<VW_SJMX> tempList = new List<VW_SJMX>();
            IEnumerator<VW_SJMX> currentEnumerator = ret.GetEnumerator();
            if (currentEnumerator != null)
            {
                for (int count = 0; currentEnumerator.MoveNext(); count++)
                {
                    tempList.Add(currentEnumerator.Current);
                }
            }

            return new PagedResultDto<VW_SJMX>(
                totalCount,
                tempList
            );

        }


        public int GetSjmx3()
        {
            //Expression.IfThen()

            Expression condition = null;
            ParameterExpression param = Expression.Parameter(typeof(Person), "p");
            Expression right = Expression.Constant("Name");
            Expression left = Expression.Property(param, typeof(Person).GetProperty("Name"));
            Expression filter = Expression.Equal(left, right);
            condition = filter;
            condition = Expression.And(condition, filter);
                        
            var totalCount = _personDapperRepository.Count(Expression.Lambda<Func<Person, bool>>(filter, param)); // ok

            return totalCount;

        }

        /// <summary>
        ///     获取Person的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<PersonListDto>> GetPagedPersons(GetPersonsInput input)
        {
            var query = _personRepository.GetAll().Include(a => a.PhoneNumbers).WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                a => a.Name.Contains(input.Filter) || a.Address.Contains(input.Filter) ||
                     a.EmailAddress.Contains(input.Filter));
            //TODO:根据传入的参数添加过滤条件
            var personCount = await query.CountAsync();

            var persons = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            //var personListDtos = ObjectMapper.Map<List <PersonListDto>>(persons);
            var personListDtos = persons.MapTo<List<PersonListDto>>();

            return new PagedResultDto<PersonListDto>(
                personCount,
                personListDtos
            );
        }

        /// <summary>
        ///     通过指定id获取PersonListDto信息
        /// </summary>
        public async Task<PersonListDto> GetPersonByIdAsync(EntityDto<int> input)
        {
            var entity = await _personRepository.GetAsync(input.Id);

            return entity.MapTo<PersonListDto>();
        }

        /// <summary>
        ///     导出Person为excel表
        /// </summary>
        /// <returns></returns>
        /// <summary>
        ///     MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetPersonForEditOutput> GetPersonForEdit(NullableIdDto<int> input)
        {
            var output = new GetPersonForEditOutput();
            PersonEditDto personEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _personRepository.GetAsync(input.Id.Value);

                personEditDto = entity.MapTo<PersonEditDto>();

                //personEditDto = ObjectMapper.Map<List <personEditDto>>(entity);
            }
            else
            {
                personEditDto = new PersonEditDto();
            }

            output.Person = personEditDto;
            return output;
        }

        /// <summary>
        ///     添加或者修改Person的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdatePerson(CreateOrUpdatePersonInput input)
        {
            if (input.Person.Id.HasValue)
                await UpdatePersonAsync(input.Person);
            else
                await CreatePersonAsync(input.Person);
        }

        /// <summary>
        ///     删除Person信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(PersonAppPermissions.Person_DeletePerson)]
        public async Task DeletePerson(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _personRepository.DeleteAsync(input.Id);
        }

        /// <summary>
        ///     批量删除Person的方法
        /// </summary>
        [AbpAuthorize(PersonAppPermissions.Person_BatchDeletePersons)]
        public async Task BatchDeletePersonsAsync(List<int> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _personRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        ///     为联系人添加电话号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PhoneNumberListDto> AddPhoneNumberAsync(PhoneNumberEditDto input)
        {
            var person = await _personRepository.GetAsync(input.PersonId);

            await _personRepository.EnsureCollectionLoadedAsync(person, p => p.PhoneNumbers);

            var phoneNumber = ObjectMapper.Map<PhoneNumber>(input);

            person.PhoneNumbers.Add(phoneNumber);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<PhoneNumberListDto>(phoneNumber);
        }

        /// <summary>
        ///     删除电话号码信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeletePhoneNumberAsync(EntityDto<long> input)
        {
            var entity = await _phoneNumbeRepository.GetAsync(input.Id);
            if (entity != null)
            {
                await _phoneNumbeRepository.DeleteAsync(entity);
            }
            else
            {
                throw new UserFriendlyException("该电话号码不存在，请重试");

            }


        }

        /// <summary>
        ///     新增Person
        /// </summary>
        [AbpAuthorize(PersonAppPermissions.Person_CreatePerson)]
        protected virtual async Task<PersonEditDto> CreatePersonAsync(PersonEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增
            var entity = ObjectMapper.Map<Person>(input);

            entity = await _personRepository.InsertAsync(entity);
            return entity.MapTo<PersonEditDto>();
        }

        /// <summary>
        ///     编辑Person
        /// </summary>
        [AbpAuthorize(PersonAppPermissions.Person_EditPerson)]
        protected virtual async Task UpdatePersonAsync(PersonEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新
            var entity = await _personRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _personRepository.UpdateAsync(entity);
        }
   }

}