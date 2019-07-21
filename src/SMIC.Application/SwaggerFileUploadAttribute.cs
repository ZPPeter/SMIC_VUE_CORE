using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMIC.Web.Host
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class SwaggerFileUploadAttribute : Attribute
    {
        public bool Required { get; private set; }

        public SwaggerFileUploadAttribute(bool Required = true)
        {
            this.Required = Required;
        }
    }
}
