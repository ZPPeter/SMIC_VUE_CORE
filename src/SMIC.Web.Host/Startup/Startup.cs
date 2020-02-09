using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Castle.Facilities.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Abp.AspNetCore;
//using Abp.Castle.Logging.Log4Net;
using Abp.Castle.Logging.NLog;
using Abp.Extensions;
using SMIC.Configuration;
using SMIC.Identity;

using Abp.AspNetCore.SignalR.Hubs;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;

using Exceptionless;
using SMIC.Utils;
using SMIC.Web.Host.Hubs;
namespace SMIC.Web.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
            _hostingEnvironment = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            // 注入 Exceptionless
            services.AddSingleton<SMIC.Utils.ILogger, ExceptionlessLogger>();
            
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            // MVC
            services.AddMvc(
                options => options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName))
            );

            services.Configure<PluginsOptions>(_appConfiguration.GetSection("Plugins"));

            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);

            services.AddSignalR();

            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {                
                options.SwaggerDoc("v1", new Info { Title = "SMIC API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                options.OperationFilter<SwaggerFileUploadFilter>(); //Swagger选项过滤器代码 ,不起作用, MVC用 ???
                /*
                   Action中的参数设置特性，测试。
                   public void TestSwaggerUploadFile([SwaggerFileUpload] file){ }
                */

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
            });

            services.AddSingleton<IConfiguration>(_appConfiguration); // 暴露给 DLL 插件用

            // 在ASP.NET Core中怎么使用 HttpContext.Current
            // https://www.cnblogs.com/maxzhang1985/p/6186455.html
            // services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            /*
            return services.AddAbp<SMICWebHostModule>(
                // Configure Log4Net logging
                options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    //f => f.UseAbpLog4Net().WithConfig("log4net.config")
                    f => f.UseAbpNLog().WithConfig("nlog.config")
                )
            );
            */

            // Configure Abp and Dependency Injection
            return services.AddAbp<SMICWebHostModule>(options =>
            {
                //Configure nLog logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                                    //f => f.UseAbpLog4Net().WithConfig("log4net.config")
                                    f => f.UseAbpNLog().WithConfig("nlog.config")
                                );

                //获取应用程序所在目录的2种方式（绝对,不受工作目录影响,建议采用此方法获取路径）.如:d:\Users\xk\Desktop\WebApplication1\WebApplication1\bin\Debug\netcoreapp2.0\
                String basePath1 = AppContext.BaseDirectory;
                String basePath2 = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                                

                //注意:对于 Linux 或非 Windows 操作系统，文件名和路径区分大小写。 例如，“SwaggerDemo.xml”文件在 Windows 上有效，但在 CentOS 上无效。
                //String path = Path.Combine(basePath2, "SwaggerDemo.xml");

                options.PlugInSources.Add(new Abp.PlugIns.FolderPlugInSource(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/", "Plugins")));// 固定位置用 @"C:\MyPlugIns"

                //options.PlugInSources.Add(new Abp.PlugIns.FolderPlugInSource(Path.Combine(basePath2, "Plugins")));// 固定位置用 @"C:\MyPlugIns"
                //options.PlugInSources.Add(new Abp.PlugIns.FolderPlugInSource(Path.Combine(_hostingEnvironment.ContentRootPath, "Plugins")));// @"C:\MyPlugIns"
                //options.PlugInSources.AddFolder(Path.Combine(_hostingEnvironment.ContentRootPath, "Areas/Plugins"), SearchOption.AllDirectories);

            });
        }                   

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(options => { options.UseAbpRequestLocalization = false; }); // Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAbpRequestLocalization();


            app.UseSignalR(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");                
                routes.MapHub<MyChatHub>("/signalr-myChatHub"); // Prefix with '/signalr'
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(_appConfiguration["App:ServerRootAddress"].EnsureEndsWith('/') + "swagger/v1/swagger.json", "SMIC API V1");
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("SMIC.Web.Host.wwwroot.swagger.ui.index.html");
            }); // URL: /swagger
            
            app.UseExceptionless(_appConfiguration); // 来自于 class ExceptionlessBuilderExtensions.cs

            /*
            // 直接使用
            ExceptionlessClient.Default.CreateLog("日志信息 2019 v3", Exceptionless.Logging.LogLevel.Debug).AddTags("tag10", "tag11").Submit();
            ExceptionlessLogger _logger = new ExceptionlessLogger();
            Exception ex = new Exception("Hello,World 2019 v3");
            ex.ToExceptionless().AddTags("tag1", "tag2").Submit();
            ex.ToExceptionless().Submit();
            _logger.Info("Test msg v3", "tag21", "tag22");
            */
        }

    }
}
