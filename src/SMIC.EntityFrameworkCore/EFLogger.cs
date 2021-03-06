﻿using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace SMIC.Utils
{
    public class EFLogger : ILogger
    {
        /*
        protected string categoryName;

        public EFLogger(string categoryName)
        {
            this.categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //通过Debugger.Log方法来将EF Core生成的Log信息输出到Visual Studio的输出窗口
            //内容很多，不停歇的输出
            Debugger.Log(0, categoryName, "=============================== EF Core log started ===============================\r\n");
            Debugger.Log(0, categoryName, formatter(state, exception) + "\r\n");
            Debugger.Log(0, categoryName, "=============================== EF Core log finished ===============================\r\n");
        }
        */

        private readonly string categoryName;

        public EFLogger(string categoryName) => this.categoryName = categoryName;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //ef core执行数据库查询时的categoryName为Microsoft.EntityFrameworkCore.Database.Command,日志级别为Information
            if (categoryName == "Microsoft.EntityFrameworkCore.Database.Command" && logLevel == LogLevel.Information)
            {
                var logContent = formatter(state, exception);

                // Console.ForegroundColor = ConsoleColor.Green;  // 无效

                //if (logContent.Contains("AbpUser") || logContent.Contains("MemberUser") )
                if (!logContent.Contains("[AbpBackgroundJobs]") && !logContent.Contains("[AbpAuditLogs]") && !logContent.Contains("[AbpEditions]") && !logContent.Contains("[AbpLanguages]") && !logContent.Contains("[AbpRoles]") && !logContent.Contains("[AbpPermissions]") && !logContent.Contains("[AbpSettings]")) {
                    Console.WriteLine("EF Logger-----------------------------------------------------------------------");
                    Console.WriteLine(logContent);
                    Console.WriteLine("EF Logger-----------------------------------------------------------------------");
                }                    

                // Console.ResetColor(); // 无效
            }
        }

        public IDisposable BeginScope<TState>(TState state) => null;


    }
}