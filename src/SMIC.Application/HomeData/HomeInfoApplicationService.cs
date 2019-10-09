
using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Abp.UI;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;


using SMIC.HomeData;
using SMIC.HomeData.Dtos;
using SMIC.HomeData.DomainService;
using SMIC.HomeData.Authorization;
using Abp.Authorization.Users;
using Abp.Dapper.Repositories;
using SMIC.Members;

namespace SMIC.HomeData
{
    /// <summary>
    /// HomeInfo应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class HomeInfoAppService : SMICAppServiceBase, IHomeInfoAppService
    {
        private readonly IRepository<HomeInfo, int> _entityRepository;
        private readonly IDapperRepository<AbpUser, long> _userRepository;

        private readonly IHomeInfoManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public HomeInfoAppService(
        IRepository<HomeInfo, int> entityRepository
        ,IHomeInfoManager entityManager, IDapperRepository<AbpUser, long> userRepository
        )
        {
            _entityRepository = entityRepository; 
             _entityManager = entityManager;
            _userRepository = userRepository;
        }


        /// <summary>
        /// 获取HomeInfo的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		// [AbpAuthorize(HomeInfoPermissions.Query)] 
        public async Task<PagedResultDto<HomeInfoListDto>> GetPaged(GetHomeInfosInput input)
		{
            Expression<Func<HomeInfo, bool>> predicate = p => (p.Id != 1);
            if (!input.FilterText.IsNullOrWhiteSpace())            
            {
                predicate = predicate.And(p => (p.Title.Contains(input.FilterText) || p.Description.Contains(input.FilterText)));
            }

            if (input.LastDate.HasValue)
            {
                predicate = predicate.And(p => p.CreationTime>= input.LastDate);
            }

            var totalCount = _entityRepository.Count(predicate);

            // .Skip(1) // 第一条记录给 HomeInfo 用, Skip 排序后不可用
            // Id = 1 表示第一条数据，不要删除！！！  -> .Where(r=>r.Id!=1)

            var entityList = await _entityRepository.GetAll()
                .Where(r=>r.Id!=1)
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(),r=>(r.Title.Contains(input.FilterText) || r.Description.Contains(input.FilterText)))
                .WhereIf(input.LastDate.HasValue, r => r.CreationTime > input.LastDate)
                .OrderByDescending(t=>t.Id)
                //.OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
				.ToListAsync();

			var entityListDtos = ObjectMapper.Map<List<HomeInfoListDto>>(entityList);
			//var entityListDtos = entityList.MapTo<List<HomeInfoListDto>>();
            
            return new PagedResultDto<HomeInfoListDto>(totalCount,entityListDtos); // 第一条记录给 HomeInfo 用
        }
                
        public async Task<PagedResultDto<HomeInfoListDto>> GetPagedNoReadNotice(GetHomeInfosInput input)
        {
            Expression<Func<HomeInfo, bool>> predicate = p => (p.Id != 1);
            input.LastDate = _userRepository.Get((long)AbpSession.UserId).ReadLastNoticeTime;
            predicate = predicate.And(p => p.CreationTime >= input.LastDate);

            var totalCount = _entityRepository.Count(predicate);

            var entityList = await _entityRepository.GetAll()
                .Where(r => r.Id != 1)
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), r => (r.Title.Contains(input.FilterText) || r.Description.Contains(input.FilterText)))
                .WhereIf(input.LastDate.HasValue, r => r.CreationTime > input.LastDate)
                .OrderByDescending(t => t.Id)
                //.OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();
            var entityListDtos = ObjectMapper.Map<List<HomeInfoListDto>>(entityList);
            return new PagedResultDto<HomeInfoListDto>(totalCount, entityListDtos); // 第一条记录给 HomeInfo 用            
        }

        public int GetNoReadNoticeCount()
        {
            Expression<Func<HomeInfo, bool>> predicate = p => (p.Id != 1);
            predicate = predicate.And(p => p.CreationTime >= _userRepository.Get((long)AbpSession.UserId).ReadLastNoticeTime);
            return _entityRepository.Count(predicate);            
        }

        /// <summary>
        /// 通过指定id获取HomeInfoListDto信息
        /// </summary>
        // [AbpAuthorize(HomeInfoPermissions.Query)] 
        public async Task<HomeInfoListDto> GetById(EntityDto<int> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    //return entity.MapTo<HomeInfoListDto>();
            return ObjectMapper.Map<HomeInfoListDto>(entity);
        }
        // 第一条记录给 HomeInfo 用
        public HomeInfo GetFirst()
        {
            var entity = _entityRepository.FirstOrDefault(1);

            //return entity.MapTo<HomeInfoListDto>();
            return ObjectMapper.Map<HomeInfo>(entity);
        }

        /// <summary>
        /// 获取编辑 HomeInfo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(HomeInfoPermissions.Create,HomeInfoPermissions.Edit)]
        public async Task<GetHomeInfoForEditOutput> GetForEdit(NullableIdDto<int> input)
		{
			var output = new GetHomeInfoForEditOutput();
            HomeInfoEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = ObjectMapper.Map<HomeInfoEditDto>(entity);

				//homeInfoEditDto = ObjectMapper.Map<List<homeInfoEditDto>>(entity);
			}
			else
			{
				editDto = new HomeInfoEditDto();
			}

			output.HomeInfo = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改HomeInfo的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		// [AbpAuthorize(HomeInfoPermissions.Create,HomeInfoPermissions.Edit)]
		public async Task CreateOrUpdate(CreateOrUpdateHomeInfoInput input)
		{

			if (input.HomeInfo.Id.HasValue)
			{
				await Update(input.HomeInfo);
			}
			else
			{
				await Create(input.HomeInfo);
			}
		}


		/// <summary>
		/// 新增HomeInfo
		/// </summary>
		// [AbpAuthorize(HomeInfoPermissions.Create)]
		protected virtual async Task<HomeInfoEditDto> Create(HomeInfoEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<HomeInfo>(input);
            //var entity=input.MapTo<HomeInfo>();
			
			entity = await _entityRepository.InsertAsync(entity);
			return ObjectMapper.Map<HomeInfoEditDto>(entity);
		}

        /// <summary>
        /// 编辑HomeInfo
        /// </summary>
        //[AbpAuthorize(HomeInfoPermissions.Edit)]
        public async Task Update(HomeInfoEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);

            //input.MapTo(entity);
			ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除HomeInfo信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		// [AbpAuthorize(HomeInfoPermissions.Delete)]
		public async Task Delete(EntityDto<int> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除HomeInfo的方法
		/// </summary>
		// [AbpAuthorize(HomeInfoPermissions.BatchDelete)]
		public async Task BatchDelete(List<int> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出HomeInfo为excel表,等待开发。
		/// </summary>
		/// <returns></returns>
		//public async Task<FileDto> GetToExcel()
		//{
		//	var users = await UserManager.Users.ToListAsync();
		//	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
		//	await FillRoleNames(userListDtos);
		//	return _userListExcelExporter.ExportToFile(userListDtos);
		//}

    }
}


