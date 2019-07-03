using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using SMIC.Authorization;

using SMIC.PhoneBooks.Persons.Authorization;
using SMIC.PhoneBooks.Persons.Dtos.LTMAutoMapper;
using SMIC.MyTasks.Mapper;
using SMIC.MyTasks.Authorization;
namespace SMIC
{
    [DependsOn(
        typeof(SMICCoreModule), 
        typeof(AbpAutoMapperModule)
        )]
    public class SMICApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<SMICAuthorizationProvider>();

            Configuration.Authorization.Providers.Add<PersonAppAuthorizationProvider>();  // SMIC.PhoneBooks.Persons.Authorization
            Configuration.Authorization.Providers.Add<TaskAuthorizationProvider>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(configuration =>
            {
                CustomerPersonMapper.CreateMappings(configuration);   // 自定义类型映射 - SMIC.PhoneBooks.Persons.Dtos.LTMAutoMapper
                TaskMapper.CreateMappings(configuration);       // SMIC.Members.Mapper/MemberUserMapper
            });
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(SMICApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );

            IocManager.RegisterAssemblyByConvention(typeof(SMICApplicationModule).GetAssembly());
            
        }
    }
}
