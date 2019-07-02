using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using SMIC.Authorization;

/*
using Abp.Dapper;
using System.Reflection;
using System.Collections.Generic;
*/
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
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(SMICApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );

            //DapperExtensions.DapperExtensions.SetMappingAssemblies(new List<Assembly> { typeof(SMICApplicationModule).GetAssembly() });
            
        }
    }
}
