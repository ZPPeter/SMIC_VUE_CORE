using System;
using Exceptionless;
namespace SMIC.Utils
{
    public class ExceptionEx
    {
        public static void SendException(string message, params string[] tags) {
            Exception ex = new Exception(message);
            ex.ToExceptionless().AddTags(tags).Submit();
            ex.ToExceptionless().Submit();
        }
    }
}
