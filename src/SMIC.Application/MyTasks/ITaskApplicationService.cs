
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
using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using Abp.Application.Services;
using Abp.Application.Services.Dto;


using SMIC.MyTasks.Dtos;
using SMIC.MyTasks;

namespace SMIC.MyTasks
{
    /// <summary>
    /// Task应用层服务的接口方法
    ///</summary>
    public interface ITaskAppService : IApplicationService
    {
        /// <summary>
		/// 获取Task的分页列表信息
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<TaskListDto>> GetPaged(GetTasksInput input);


		/// <summary>
		/// 通过指定id获取TaskListDto信息
		/// </summary>
		Task<TaskListDto> GetById(EntityDto<int> input);


        /// <summary>
        /// 返回实体的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetTaskForEditOutput> GetForEdit(NullableIdDto<int> input);


        /// <summary>
        /// 添加或者修改Task的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateTaskInput input);


        /// <summary>
        /// 删除Task信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<int> input);


        /// <summary>
        /// 批量删除Task
        /// </summary>
        Task BatchDelete(List<int> input);


		/// <summary>
        /// 导出Task为excel表
        /// </summary>
        /// <returns></returns>
		//Task<FileDto> GetToExcel();

    }
}
