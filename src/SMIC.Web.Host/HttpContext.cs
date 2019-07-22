using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMIC.Web.Host
{
    public static class HttpContext  // 模拟一个HttpContext.Current
    {
        public static IServiceProvider ServiceProvider;

        public static Microsoft.AspNetCore.Http.HttpContext Current
        {
            get
            {
                object factory = ServiceProvider.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));
                Microsoft.AspNetCore.Http.HttpContext context = ((Microsoft.AspNetCore.Http.HttpContextAccessor)factory).HttpContext;
                return context;
            }
        }

    }
}
