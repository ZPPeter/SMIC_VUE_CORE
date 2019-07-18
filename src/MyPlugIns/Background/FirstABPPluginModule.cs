using Abp.Modules;
using Abp.Threading.BackgroundWorkers;
using Abp.AutoMapper;
using System.Configuration;
using Microsoft.Extensions.Configuration; //.net core
using SMIC;
using Abp.Reflection.Extensions;
using Abp.AspNetCore.Configuration;

namespace TestPlugIn
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
                
        public override void PostInitialize()
        {            
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<DeleteOldAuditLogsWorker>());
        }

        public override void PreInitialize()
        {
            TestModule.Hello = "888888";

            // SMIC.Web.Host.dll.config
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string a = config.AppSettings.Settings["owin:AutomaticAppStartup"].Value;
            
            //注册权限
            Configuration.Authorization.Providers.Add<Authorization.PlugInZeroAuthorizationProvider>();

            //设置生成webapi
            Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(FirstABPPluginModule).Assembly, moduleName: "app2", useConventionalHttpVerbs: true);

        }
    }
}

/*

//插件配置(直接从当前执行的程序集的config文件读取数据库连接串)
var config = ConfigurationManager.OpenExeConfiguration(
    Assembly.GetExecutingAssembly().Location);
string connectStr = config.ConnectionStrings.ConnectionStrings["PlugInZeroDB"].ConnectionString;
//注册DbContext,构建时使用指定参数
IocManager.IocContainer.Register(
    Component.For<PlugInZeroDbContext>()
    .DependsOn(
        Dependency.OnValue(
            "connectionString", connectStr))); 

*/
