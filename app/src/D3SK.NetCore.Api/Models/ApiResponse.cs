using System.Collections.Generic;
using D3SK.NetCore.Common.Models;

namespace D3SK.NetCore.Api.Models
{
    public class ApiResponseCodes
    {
        public const int NotSet = 0;

        public const int Success = 1;
        
        public const int PartialSuccess = 2;

        public const int Exception = 3;

        public const int UncaughtException = 4;
    }

    public class ApiResponse
    {
        public int Code { get; set; } = ApiResponseCodes.Success;

        public object Data { get; set; }

        public IList<ExceptionMessage> Messages { get; set; } = new List<ExceptionMessage>();
    }
}
