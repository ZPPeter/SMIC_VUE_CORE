using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

//Swagger选项过滤器代码
// IFromFile -> 文件选择按钮，使用之前是 file{}
/*
 实现原理，通过swagger提供的filter，找到action中带有SwaggerFileUpload特性的参数，
 然后给swagger operaion.parameters添加一个自定义的参数，
 即文件类型参数即可。

https://www.cnblogs.com/intotf/p/10075162.html
前提：
 需要nuget   Swashbuckle.AspNetCore 我暂时用的是  4.01 最新版本；
 描述：解决 .net core webapi 上传文件使用的是 IFormFile，在Swagger 接口描叙的时候很不友好，为解决接口文档的友好描叙；
*/
namespace SMIC.Web.Host
{
    /// <summary>
    /// Swagger 上传文件过滤器
    /// </summary>
    public class SwaggerFileUploadFilter : IOperationFilter
    {
        private static readonly string[] FormFilePropertyNames =
        typeof(IFormFile).GetTypeInfo().DeclaredProperties.Select(x => x.Name).ToArray();
        public void Apply(Operation operation, OperationFilterContext context)
        {

            if (!context.ApiDescription.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase) &&
               !context.ApiDescription.HttpMethod.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var fileParameters = context.ApiDescription.ActionDescriptor.Parameters.Where(n => n.ParameterType == typeof(IFormFile)).ToList();

            if (fileParameters.Count < 0)
            {
                return;
            }

            foreach (var fileParameter in fileParameters)
            {
                var formFileParameters = operation
                .Parameters
                .OfType<NonBodyParameter>()
                .Where(x => FormFilePropertyNames.Contains(x.Name))
                .ToArray();
                foreach (var formFileParameter in formFileParameters)
                {
                    operation.Parameters.Remove(formFileParameter);
                }

                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = fileParameter.Name,
                    In = "formData",
                    Description = "文件上传",
                    Required = true,
                    Type = "file"
                });
                operation.Consumes.Add("multipart/form-data");
            }
        }
    }

}

/*
/// <summary>
/// swagger file upload parameter filter
/// </summary>
public class SwaggerFileUploadFilter : IOperationFilter
{
public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
{
var parameters = apiDescription.ActionDescriptor.GetParameters();
foreach (HttpParameterDescriptor parameterDesc in parameters)
{
var fileUploadAttr = parameterDesc.GetCustomAttributes<SwaggerFileUploadAttribute>().FirstOrDefault();
if (fileUploadAttr != null)
{
operation.consumes.Add("multipart/form-data");
 
operation.parameters.Add(new Parameter
{
name = parameterDesc.ParameterName + "_file",
@in = "formData",
description = "file to upload",
required = fileUploadAttr.Required,
type = "file"
});
}
}
}
} 
*/
