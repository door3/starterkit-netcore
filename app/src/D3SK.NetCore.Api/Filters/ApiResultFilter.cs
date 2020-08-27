using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using D3SK.NetCore.Api.Controllers;
using D3SK.NetCore.Api.Models;

namespace D3SK.NetCore.Api.Filters
{
    public class ApiResultFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            if (context.ActionContext.ControllerContext.ControllerDescriptor.ControllerType == typeof(ApiResponseControllerBase))
            {
                var controller = context.ActionContext.ControllerContext.Controller as ApiResponseControllerBase;
                context.Response.TryGetContentValue(out object data);

                var apiResponse = new ApiResponse
                {
                    Code = controller.ResponseCode,
                    Data = data,
                    Messages = controller.ExceptionManager.Messages
                };

                var statusCode = context.Response.StatusCode;
                context.Response = context.Request.CreateResponse(statusCode, apiResponse);
            }

            base.OnActionExecuted(context);
        }
    }
}
