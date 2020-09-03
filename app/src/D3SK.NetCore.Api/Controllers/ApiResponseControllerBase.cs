using D3SK.NetCore.Api.Models;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace D3SK.NetCore.Api.Controllers
{
    [ApiController]
    public class ApiResponseControllerBase : ControllerBase
    {
        public int ResponseCode { get; set; } = ApiResponseCodes.Success;

        public IExceptionManager ExceptionManager { get; }

        public ApiResponseControllerBase(IExceptionManager exceptionManager)
        {
            ExceptionManager = exceptionManager.NotNull(nameof(ExceptionManager));
        }

        protected IActionResult Forbidden()
        {
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }
}
