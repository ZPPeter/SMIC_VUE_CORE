using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using SMIC.Authorization;

using SMIC.PhoneBooks.Persons.Authorization;
using SMIC.PhoneBooks.Persons.Dtos.LTMAutoMapper;
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

            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomerPersonMapper.CreateMappings); // SMIC.PhoneBooks.Persons.Dtos.LTMAutoMapper

        }

        public override void Initialize()
        {
            var thisAssembly = typeof(SMICApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
