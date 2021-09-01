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
using D3SK.NetCore.Infrastructure;
using D3SK.NetCore.Api.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using D3SK.NetCore.Domain.Models;
using D3SK.NetCore.Infrastructure.Exceptions;

namespace D3SK.NetCore.Api.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute, IFilterMetadata
    {
        private readonly ILogger<ApiExceptionFilter> _logger;
        private readonly IOptions<DomainOptions> _domainOptions;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger, IOptions<DomainOptions> domainOptions)
        {
            _logger = logger.NotNull(nameof(logger));
            _domainOptions = domainOptions.NotNull(nameof(domainOptions));
        }

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
            else if (context.Exception is FileNotFoundException || context.Exception is EntityNotFoundException)
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
            else if (context.Exception is DomainException)
            {
                var innerEx = context.Exception.GetInnermostException();
                messages.Add(new ErrorMessage(innerEx.Message, ExceptionMessageTypes.DefaultErrorCode, innerEx.StackTrace));
                code = ApiResponseCodes.UncaughtException;
            }
            else
            {
                if (_domainOptions.Value.ShowExtendedError)
                {
                    var innerEx = context.Exception.GetInnermostException();
                    messages.Add(new ErrorMessage(innerEx.Message, ExceptionMessageTypes.DefaultErrorCode, innerEx.StackTrace));
                }
                else
                {
                    messages.Add(new ErrorMessage("Internal server error", ExceptionMessageTypes.DefaultErrorCode));
                }

                code = ApiResponseCodes.UncaughtException;
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
