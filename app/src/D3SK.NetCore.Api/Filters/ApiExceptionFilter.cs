using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using D3SK.NetCore.Api.Models;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using ExceptionFilterAttribute = System.Web.Http.Filters.ExceptionFilterAttribute;
using IExceptionFilter = System.Web.Http.Filters.IExceptionFilter;

namespace D3SK.NetCore.Api.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute, IFilterMetadata
    {
        public override void OnException(HttpActionExecutedContext context)
        { 
            var messages = new List<ExceptionMessage>();
            object tempData;
            var code = ApiResponseCodes.Exception;

            if (context.Exception is ThrownWithMessagesException ex)
            {
                messages = ex.Messages.ToList();
                tempData = ex.TempData;
            }
            else
            {
                var innerEx = context.Exception.GetInnermostException();
                messages.Add(new ErrorMessage(innerEx.Message, ExceptionMessageTypes.DefaultErrorCode, innerEx.StackTrace));
                tempData = context.Exception.ToTempData();
                code = ApiResponseCodes.UncaughtException;
            }

            var apiResponse = new ApiResponse()
            {
                Code = code,
                Data = tempData,
                Messages = messages
            };

            context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, apiResponse);
        }
    }
}
