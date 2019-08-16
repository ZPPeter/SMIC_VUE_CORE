
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


namespace SMIC.HomeData
{
    /// <summary>
    /// HomeInfo应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class HomeInfoAppService : SMICAppServiceBase, IHomeInfoAppService
    {
        private readonly IRepository<HomeInfo, int> _entityRepository;

        private readonly IHomeInfoManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public HomeInfoAppService(
        IRepository<HomeInfo, int> entityRepository
        ,IHomeInfoManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取HomeInfo的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[AbpAuthorize(HomeInfoPermissions.Query)] 
        public async Task<PagedResultDto<HomeInfoListDto>> GetPaged(GetHomeInfosInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<HomeInfoListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<HomeInfoListDto>>();

			return new PagedResultDto<HomeInfoListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取HomeInfoListDto信息
		/// </summary>
		// [AbpAuthorize(HomeInfoPermissions.Query)] 
		public async Task<HomeInfoListDto> GetById(EntityDto<int> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return entity.MapTo<HomeInfoListDto>();
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

				editDto = entity.MapTo<HomeInfoEditDto>();

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
		[AbpAuthorize(HomeInfoPermissions.Create,HomeInfoPermissions.Edit)]
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
		[AbpAuthorize(HomeInfoPermissions.Create)]
		protected virtual async Task<HomeInfoEditDto> Create(HomeInfoEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <HomeInfo>(input);
            var entity=input.MapTo<HomeInfo>();
			

			entity = await _entityRepository.InsertAsync(entity);
			return entity.MapTo<HomeInfoEditDto>();
		}

        /// <summary>
        /// 编辑HomeInfo
        /// </summary>
        //[AbpAuthorize(HomeInfoPermissions.Edit)]
        public async Task Update(HomeInfoEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			input.MapTo(entity);

			// ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除HomeInfo信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[AbpAuthorize(HomeInfoPermissions.Delete)]
		public async Task Delete(EntityDto<int> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}



		/// <summary>
		/// 批量删除HomeInfo的方法
		/// </summary>
		[AbpAuthorize(HomeInfoPermissions.BatchDelete)]
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


