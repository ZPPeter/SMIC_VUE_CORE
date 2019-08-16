using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMIC.Utils
{
    using Exceptionless;
    using Exceptionless.Logging;
    public class ExceptionlessLogger : ILogger
    {
        public void Debug(string message, params string[] tags)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Debug).AddTags(tags).Submit();
        }

        public void Error(string message, params string[] tags)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Error).AddTags(tags).Submit();
        }

        public void Fatal(string message, params string[] tags)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Fatal).AddTags(tags).Submit();
        }

        public void Info(string message, params string[] tags)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Info).AddTags(tags).Submit();
        }

        public void Off(string message, params string[] tags)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Off).AddTags(tags).Submit();
        }

        public void Other(string message, params string[] tags)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Other).AddTags(tags).Submit();
        }

        public void Trace(string message, params string[] tags)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Trace).AddTags(tags).Submit();
        }

        public void Warn(string message, params string[] tags)
        {
            ExceptionlessClient.Default.CreateLog(message, LogLevel.Warn).AddTags(tags).Submit();
        }
    }

}
