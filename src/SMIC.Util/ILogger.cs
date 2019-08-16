using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMIC.Utils
{
    public interface ILogger
    {
        void Debug(string message, params string[] tags);
        void Error(string message, params string[] tags);
        void Fatal(string message, params string[] tags);
        void Info(string message, params string[] tags);
        void Off(string message, params string[] tags);
        void Other(string message, params string[] tags);
        void Trace(string message, params string[] tags);
        void Warn(string message, params string[] tags);
    }

}
