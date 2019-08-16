
using AutoMapper;
using SMIC.HomeData;
using SMIC.HomeData.Dtos;

namespace SMIC.HomeData.Mapper
{

	/// <summary>
    /// 配置HomeInfo的AutoMapper
    /// </summary>
	internal static class HomeInfoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <HomeInfo,HomeInfoListDto>();
            configuration.CreateMap <HomeInfoListDto,HomeInfo>();

            configuration.CreateMap <HomeInfoEditDto,HomeInfo>();
            configuration.CreateMap <HomeInfo,HomeInfoEditDto>();

        }
	}
}
