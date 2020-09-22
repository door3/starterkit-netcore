using System.Collections.Generic;
using System.Linq;
using D3SK.NetCore.Api.Models;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace D3SK.NetCore.Api.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute, IFilterMetadata
    {
        private readonly ILogger<ApiExceptionFilter> _logger;
        
        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) =>
            _logger = logger.NotNull(nameof(logger));

        public override void OnException(ExceptionContext context)
        { 
            var messages = new List<ExceptionMessage>();
            object tempData;
            var code = ApiResponseCodes.Exception;

            if (context.Exception is ThrownWithMessagesException ex)
            {
                messages = ex.Messages.ToList();
                tempData = ex.TempData;

                _logger.LogTrace("An error occurred: {messages}, {tempData}", messages, tempData);
            }
            else
            {
                var innerEx = context.Exception.GetInnermostException();
                messages.Add(new ErrorMessage(innerEx.Message, ExceptionMessageTypes.DefaultErrorCode, innerEx.StackTrace));
                tempData = context.Exception.ToTempData();
                code = ApiResponseCodes.UncaughtException;

                _logger.LogError(context.Exception, "Internal server error");
            }

            var apiResponse = new ApiResponse()
            {
                Code = code,
                Data = tempData,
                Messages = messages
            };


            var result = new ObjectResult(apiResponse)
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Value = apiResponse,
                DeclaredType = typeof(ApiResponse)
            };

            context.Result = result;
        }
    }
}
