using Abp.Modules;
using Abp.Threading.BackgroundWorkers;
using Abp.AutoMapper;
using System.Configuration;
using Microsoft.Extensions.Configuration; //.net core
using SMIC;
using Abp.Reflection.Extensions;
using Abp.AspNetCore.Configuration;

namespace MyPlugIn
{
    //[DependsOn(typeof(AbpZeroCoreModule))]
    [DependsOn(
        typeof(SMICCoreModule),
        typeof(AbpAutoMapperModule)
        )]
    public class FirstABPPluginModule : AbpModule
    {        
        private readonly string _connectionString;
        public FirstABPPluginModule(IConfiguration AppConfiguration)   // 这段代码不会执行的
        {            
            string a = AppConfiguration["AbpZeroLicenseCode"];
            _connectionString = AppConfiguration.GetConnectionString("Default");
            Logger.Info("我来自插件 _connectionString:" + _connectionString);
        }

        public override void Initialize()
        {
            // IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());           
            IocManager.RegisterAssemblyByConvention(typeof(FirstABPPluginModule).GetAssembly()); // using Abp.Reflection.Extensions;
        }
                
        public override void PostInitialize()  // 启动定时操作业务,目前未用
        {            
            // var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            // workManager.Add(IocManager.Resolve<DeleteOldAuditLogsWorker>());
        }

        public override void PreInitialize()
        {
            //TestModule.Hello = "888888";

            // SMIC.Web.Host.dll.config
            //System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //string a = config.AppSettings.Settings["owin:AutomaticAppStartup"].Value;
            
            //注册权限- 目前未用
            //Configuration.Authorization.Providers.Add<Authorization.PlugInZeroAuthorizationProvider>();

            //设置生成webapi
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(FirstABPPluginModule).Assembly, moduleName: "save_error_logger", useConventionalHttpVerbs: true);

        }
    }
}