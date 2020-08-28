using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Common.Models
{
    public static class ExceptionMessageTypes
    {
        public const string Error = "error";
        public const string Warning = "warning";
        public const string Info = "info";

        public const int DefaultErrorCode = 1;
        public const int DefaultInfoCode = 0;
        public const int DefaultWarningCode = 2;
    }

    public class ExceptionMessage
    {
        public string Type { get; }

        public int Code { get; set; }

        public string Message { get; set; }

        public object ExtendedData { get; set; }

        public ExceptionMessage(string type, int code, string message = null,
            object extendedData = null)
        {
            Type = type.NotNull(nameof(type));
            Code = code;
            Message = message;
            ExtendedData = extendedData;
        }
    }

    public class ErrorMessage : ExceptionMessage
    {
        public ErrorMessage(string message, int code = ExceptionMessageTypes.DefaultErrorCode,
            object extendedData = null) : base(
            ExceptionMessageTypes.Error, code, message, extendedData)
        {
        }
    }

    public class WarningMessage : ExceptionMessage
    {
        public WarningMessage(string message, int code = ExceptionMessageTypes.DefaultWarningCode,
            object extendedData = null) : base(
            ExceptionMessageTypes.Warning, code, message, extendedData)
        {
        }
    }

    public class InfoMessage : ExceptionMessage
    {
        public InfoMessage(string message, int code = ExceptionMessageTypes.DefaultInfoCode,
            object extendedData = null) : base(
            ExceptionMessageTypes.Info, code, message, extendedData)
        {
        }
    }
}
