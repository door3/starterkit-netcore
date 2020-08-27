using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using D3SK.NetCore.Api.Controllers;
using D3SK.NetCore.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace D3SK.NetCore.Api.Filters
{
    public class ApiResultFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Controller is ApiResponseControllerBase apiController)
            {
                if (context.Result is ObjectResult objResult)
                {
                    var apiResponse = new ApiResponse()
                    {
                        Code = apiController.ResponseCode,
                        Data = objResult.Value,
                        Messages = apiController.ExceptionManager.Messages
                    };

                    objResult.Value = apiResponse;
                    objResult.DeclaredType = typeof(ApiResponse);
                }
            }

            await next();
        }
    }
}
