using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Models;

namespace D3SK.NetCore.Common.Utilities
{
    public interface IExceptionManager
    {
        IList<ExceptionMessage> Messages { get; }

        bool HasErrors { get; }

        object TempData { get; set; }

        void AddMessage(ExceptionMessage message);

        void AddMessage(string type, string message, int code = ExceptionMessageTypes.DefaultInfoCode,
            object extendedData = null);

        void AddErrorMessage(Exception ex, int code = ExceptionMessageTypes.DefaultErrorCode,
            bool useInnermostException = true);

        void AddErrorMessage(string message, int code = ExceptionMessageTypes.DefaultErrorCode,
            object extendedData = null);

        void AddWarningMessage(string message, int code = ExceptionMessageTypes.DefaultWarningCode,
            object extendedData = null);

        void AddInfoMessage(string message, int code = ExceptionMessageTypes.DefaultInfoCode,
            object extendedData = null);

        void ThrowException(Exception ex, int code = ExceptionMessageTypes.DefaultErrorCode,
            bool useInnermostException = true);

        void Throw(bool onlyWithErrorMessage = true);

        void TryCatch(
            Action action,
            Func<Exception, bool> onException = null,
            Action finallyAction = null);

        Task TryCatchAsync(
            Func<Task> actionAsync,
            Func<Exception, bool> onException = null,
            Action finallyAction = null,
            Func<Task> finallyActionAsync = null);
    }
}
