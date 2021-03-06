
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


using SMIC.MyTasks;
using SMIC.MyTasks.Dtos;
using SMIC.MyTasks.DomainService;
using SMIC.MyTasks.Authorization;


namespace SMIC.MyTasks
{
    /// <summary>
    /// Task应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class TaskAppService : SMICAppServiceBase, ITaskAppService
    {
        private readonly IRepository<MyTask, int> _entityRepository;

        private readonly ITaskManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public TaskAppService(
        IRepository<MyTask, int> entityRepository
        ,ITaskManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取Task的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[AbpAuthorize(TaskPermissions.Query)] 
        public async Task<PagedResultDto<TaskListDto>> GetPaged(GetTasksInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			var entityListDtos = ObjectMapper.Map<List<TaskListDto>>(entityList);
			//var entityListDtos =entityList.MapTo<List<TaskListDto>>();

			return new PagedResultDto<TaskListDto>(count,entityListDtos);
		}


		/// <summary>
		/// 通过指定id获取TaskListDto信息
		/// </summary>
		[AbpAuthorize(TaskPermissions.Query)] 
		public async Task<TaskListDto> GetById(EntityDto<int> input)
		{
			var entity = await _entityRepository.GetAsync(input.Id);

		    return ObjectMapper.Map<TaskListDto>(entity);
		}

		/// <summary>
		/// 获取编辑 Task
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[AbpAuthorize(TaskPermissions.Create,TaskPermissions.Edit)]
		public async Task<GetTaskForEditOutput> GetForEdit(NullableIdDto<int> input)
		{
			var output = new GetTaskForEditOutput();
            TaskEditDto editDto;

			if (input.Id.HasValue)
			{
				var entity = await _entityRepository.GetAsync(input.Id.Value);

				editDto = ObjectMapper.Map<TaskEditDto>(entity);

				//taskEditDto = ObjectMapper.Map<List<taskEditDto>>(entity);
			}
			else
			{
				editDto = new TaskEditDto();
			}

			output.Task = editDto;
			return output;
		}


		/// <summary>
		/// 添加或者修改Task的公共方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[AbpAuthorize(TaskPermissions.Create,TaskPermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateTaskInput input)
		{
			if (input.Task.Id.HasValue)
			{
				await Update(input.Task);
			}
			else
			{
				await Create(input.Task);
			}
		}


		/// <summary>
		/// 新增Task
		/// </summary>
		[AbpAuthorize(TaskPermissions.Create)]
		protected virtual async Task<TaskEditDto> Create(TaskEditDto input)
		{
			//TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map <MyTask>(input);
            //var entity=input.MapTo<MyTask>();            

			entity = await _entityRepository.InsertAsync(entity);
			return ObjectMapper.Map<TaskEditDto>(entity);
		}

		/// <summary>
		/// 编辑Task
		/// </summary>
		[AbpAuthorize(TaskPermissions.Edit)]
		protected virtual async Task Update(TaskEditDto input)
		{
			//TODO:更新前的逻辑判断，是否允许更新

			var entity = await _entityRepository.GetAsync(input.Id.Value);
			//input.MapTo(entity);

			 ObjectMapper.Map(input, entity);
		    await _entityRepository.UpdateAsync(entity);
		}



		/// <summary>
		/// 删除Task信息的方法
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		[AbpAuthorize(TaskPermissions.Delete)]
		public async Task Delete(EntityDto<int> input)
		{
			//TODO:删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(input.Id);
		}                      

        /// <summary>
        /// 批量删除Task的方法
        /// </summary>
        [AbpAuthorize(TaskPermissions.BatchDelete)]
		public async Task BatchDelete(List<int> input)
		{
			// TODO:批量删除前的逻辑判断，是否允许删除
			await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
		}


		/// <summary>
		/// 导出Task为excel表,等待开发。
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


