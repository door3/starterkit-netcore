using System;
using System.Linq;
using System.Reflection;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace D3SK.NetCore.Api.Filters
{
    public class SwaggerExcludeFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext schemaFilterContext)
        {
            if (schema.Properties.Count == 0)
                return;

            const BindingFlags bindingFlags = BindingFlags.Public |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Instance;
            var memberList = schemaFilterContext.Type
                                .GetFields(bindingFlags).Cast<MemberInfo>()
                                .Concat(schemaFilterContext.Type
                                .GetProperties(bindingFlags));

            var excludedList = memberList
                .Where(m => m.GetCustomAttribute<UpdateStrategyAttribute>()?.NullOnAdd == true)
                .Select(m => m.Name.ToCamelCase());

            foreach (var excludedName in excludedList)
            {
                if (schema.Properties.ContainsKey(excludedName))
                {
                    schema.Properties.Remove(excludedName);
                }
            }
        }
    }
}
