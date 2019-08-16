using System;
using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace SMIC.Web.Host
{
    /*
        用到的 elasticsearch 和 kibana 都是 0 配置的，基于 java 需要 java 环境支持
        * 收费的  Exceptionless,默认 ServerUrl,ServerUrl 为空即可
        * app.UseExceptionless("zf8yUfIwRAVKqrzke8P9hsfLxDy8fucgsE2VmcYM");         
        * https://be.exceptionless.io/ 注册账号，选择项目类型
        * 免费版只能一个项目，每月3000条记录，保留3天        
    */

    public static class ExceptionlessBuilderExtensions
    {
        public static IApplicationBuilder UseExceptionless(this IApplicationBuilder app, IConfiguration configuration)
        {
            var isEnableLogger = Convert.ToBoolean(configuration["Exceptionless:Enabled"] ?? "false");
            if (isEnableLogger)
            {                
                ExceptionlessClient.Default.Configuration.ApiKey = configuration["Exceptionless:ApiKey"];
                if (!string.IsNullOrEmpty(configuration["Exceptionless:ServerUrl"]) && configuration["Exceptionless:ServerUrl"].Trim().Length!=0)
                {
                    ExceptionlessClient.Default.Configuration.ServerUrl = configuration["Exceptionless:ServerUrl"];
                }
                ExceptionlessClient.Default.SubmittingEvent += OnSubmittingEvent;
                app.UseExceptionless();
            }

            return app;
        }

        private static void OnSubmittingEvent(object sender, EventSubmittingEventArgs e)
        {
            // 只处理未处理的异常
            //if (!e.IsUnhandledError)
            //{
            //    return;
            //}

            // 忽略404错误
            if (e.Event.IsNotFound())
            {
                e.Cancel = true;
                return;
            }

            // 忽略没有错误体的错误
            var error = e.Event.GetError();
            if (error == null)
            {
                return;
            }

            // 忽略 401 (Unauthorized) 和 请求验证的错误.
            if (error.Code == "401" || error.Type == "System.Web.HttpRequestValidationException")
            {
                e.Cancel = true;
                return;
            }

            // Ignore any exceptions that were not thrown by our code.
            //var handledNamespaces = new List<string> { "Exceptionless" };
            //if (!error.StackTrace.Select(s => s.DeclaringNamespace).Distinct().Any(ns => handledNamespaces.Any(ns.Contains)))
            //{
            //    e.Cancel = true;
            //    return;
            //}

            // 添加附加信息.
            //e.Event.Tags.Add("EDC.Core");
            //e.Event.MarkAsCritical();
        }
    }
}
