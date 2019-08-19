using System;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.Configuration;
using SMIC.Authentication.JwtBearer;
using SMIC.Configuration;
using SMIC.EntityFrameworkCore;
//using MyPlugIn;
namespace SMIC
{
    [DependsOn(
         typeof(SMICApplicationModule), // SMIC.Application 应用层
         //typeof(FirstABPPluginModule),  // 第二个 应用层，目前不知道怎么用插件实现
         typeof(SMICEntityFrameworkModule),
         typeof(AbpAspNetCoreModule)
        ,typeof(AbpAspNetCoreSignalRModule)
     )]
    public class SMICWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public SMICWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                SMICConsts.ConnectionStringName
            );

            // Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(SMICApplicationModule).GetAssembly()
                 );

            //设置生成webapi
            //Configuration.Modules.AbpAspNetCore().CreateControllersForAppServices(typeof(FirstABPPluginModule).Assembly, moduleName: "cline", useConventionalHttpVerbs: true);

            ConfigureTokenAuth();

            // 相对过期时间 默认 60 min
            // DefaultSlidingExpireTime -> 多久未访问，超过多少时间不调用就失效            
            // 只要在相对过期时间内，被访问,那么永远不会过期（由于内存不足删除除外）

            // 绝对过期时间 默认 60 min
            // DefaultAbsoluteExpireTime -> 定时销毁，超过多少秒后过期，不管有没有操作缓存都过期
                        
            //配置所有Cache的默认 相对过期 时间为2小时,默认 60 min
            Configuration.Caching.ConfigureAll(
            cache =>
            {
                // cache.DefaultAbsoluteExpireTime 
                cache.DefaultSlidingExpireTime = TimeSpan.FromHours(2);
            });

            //配置指定的Cache绝对过期时间为1天，统计图表
            Configuration.Caching.Configure(
            "StatsCacheBy", cache =>
            {
                cache.DefaultAbsoluteExpireTime = TimeSpan.FromDays(1);                
            });

        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SMICWebCoreModule).GetAssembly());
        }
    }
}
