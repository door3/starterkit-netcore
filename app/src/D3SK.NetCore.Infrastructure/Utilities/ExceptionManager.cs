using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Models;
using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Infrastructure.Utilities
{
    public class ExceptionManager : IExceptionManager
    {
        public IList<ExceptionMessage> Messages { get; } = new List<ExceptionMessage>();

        public bool HasErrors => Messages.Any(m => m.Type == ExceptionMessageTypes.Error);

        public object TempData { get; set; }

        public void AddMessage(ExceptionMessage message)
        {
            Messages.Add(message);
        }

        public void AddMessage(string type, string message, int code = ExceptionMessageTypes.DefaultInfoCode,
            object extendedData = null)
        {
            Messages.Add(new ExceptionMessage(type, code, message, extendedData));
        }

        public void AddErrorMessage(Exception ex, int code = ExceptionMessageTypes.DefaultErrorCode,
            bool useInnermostException = true)
        {
            if (useInnermostException)
            {
                ex = ex.GetInnermostException();
            }

            Messages.Add(new ErrorMessage(ex.Message, code, ex.ToTempData()));
        }

        public void AddErrorMessage(string message, int code = ExceptionMessageTypes.DefaultErrorCode,
            object extendedData = null)
        {
            Messages.Add(new ErrorMessage(message, code, extendedData));
        }

        public void AddWarningMessage(string message, int code = ExceptionMessageTypes.DefaultWarningCode,
            object extendedData = null)
        {
            Messages.Add(new WarningMessage(message, code, extendedData));
        }

        public void AddInfoMessage(string message, int code = ExceptionMessageTypes.DefaultInfoCode,
            object extendedData = null)
        {
            Messages.Add(new InfoMessage(message, code, extendedData));
        }

        public virtual void ThrowException(Exception ex, int code = ExceptionMessageTypes.DefaultErrorCode,
            bool useInnermostException = true)
        {
            AddErrorMessage(ex, code, useInnermostException);

            throw new ThrownWithMessagesException(Messages.ToImmutableList(), TempData);
        }

        public virtual void Throw(bool onlyWithErrorMessage = true)
        {
            if (onlyWithErrorMessage && !HasErrors)
            {
                return;
            }

            throw new ThrownWithMessagesException(Messages.ToImmutableList(), TempData);
        }

        public virtual void TryCatch(
            Action action,
            Func<Exception, bool> onException = null,
            Action finallyAction = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (onException == null || onException(ex))
                {
                    ThrowException(ex);
                }
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }

        public virtual async Task TryCatchAsync(
            Func<Task> actionAsync,
            Func<Exception, bool> onException = null,
            Action finallyAction = null,
            Func<Task> finallyActionAsync = null)
        {
            try
            {
                await actionAsync();
            }
            catch (Exception ex)
            {
                if (onException == null || onException(ex))
                {
                    ThrowException(ex);
                }
            }
            finally
            {
                finallyAction?.Invoke();
                if (finallyActionAsync != null)
                {
                    await finallyActionAsync();
                }
            }
        }
    }
}