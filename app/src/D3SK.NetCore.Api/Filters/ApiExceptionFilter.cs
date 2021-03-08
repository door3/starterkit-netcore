using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using D3SK.NetCore.Api.Models;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
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
            object tempData = context?.Exception?.ToTempData();
            var code = ApiResponseCodes.Exception;
            var statusCode = StatusCodes.Status500InternalServerError;

            if (context.Exception is ThrownWithMessagesException ex)
            {
                messages = ex.Messages.ToList();
                tempData = ex.TempData;

                statusCode = StatusCodes.Status400BadRequest;

                if (messages.Any(t => t.Code == StatusCodes.Status403Forbidden))
                {
                    statusCode = StatusCodes.Status403Forbidden;
                }

                _logger.LogTrace("An error occurred: {messages}, {tempData}", messages, tempData);
            }
            else if (context.Exception is FileNotFoundException)
            {
                var innerEx = context.Exception.GetInnermostException();
                messages.Add(new ErrorMessage(innerEx.Message, ExceptionMessageTypes.DefaultErrorCode, innerEx.StackTrace));
                statusCode = StatusCodes.Status404NotFound;
            }
            else if (context.Exception is DbUpdateConcurrencyException)
            {
                statusCode = StatusCodes.Status409Conflict;
                _logger.LogTrace("Concurrency error");
            }
            else
            {
                var innerEx = context.Exception.GetInnermostException();
                messages.Add(new ErrorMessage(innerEx.Message, ExceptionMessageTypes.DefaultErrorCode, innerEx.StackTrace));
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
                StatusCode = statusCode,
                Value = apiResponse,
                DeclaredType = typeof(ApiResponse)
            };

            context.Result = result;
        }
    }
}
