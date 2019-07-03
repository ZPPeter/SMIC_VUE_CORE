
using AutoMapper;
using SMIC.MyTasks;
using SMIC.MyTasks.Dtos;

namespace SMIC.MyTasks.Mapper
{

	/// <summary>
    /// 配置Task的AutoMapper
    /// </summary>
	internal static class TaskMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <MyTask,TaskListDto>();
            configuration.CreateMap <TaskListDto,MyTask>();

            configuration.CreateMap <TaskEditDto,MyTask>();
            configuration.CreateMap <MyTask,TaskEditDto>();

        }
	}
}
