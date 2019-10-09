using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using SMIC.Configuration;
//using Abp.Web.SignalR;

namespace SMIC.Web.Host.Startup
{
    //[DependsOn(typeof(AbpWebSignalRModule))]
    [DependsOn(
       typeof(SMICWebCoreModule))]
    public class SMICWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public SMICWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SMICWebHostModule).GetAssembly());
        }
    }
}
