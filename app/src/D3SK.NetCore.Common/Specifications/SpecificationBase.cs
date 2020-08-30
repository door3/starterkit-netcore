using System.ComponentModel.DataAnnotations;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Models;
using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Common.Specifications
{
    public abstract class SpecificationBase : ISpecificationBase
    {
        public IExceptionManager ExceptionManager { get; private set; }

        public string ErrorMessage { get; private set; }

        public int ErrorCode { get; private set; } = ExceptionMessageTypes.DefaultErrorCode;

        public object ExtendedData { get; private set; }

        protected virtual bool HandleSpecification(bool isSatisfied)
        {
            if (isSatisfied) return true;

            if (ErrorMessage.IsEmpty()) return false;

            if (ExceptionManager != null)
            {
                ExceptionManager.AddErrorMessage(ErrorMessage, ErrorCode, ExtendedData);
            }
            else
            {
                throw new ValidationException(ErrorMessage);
            }

            return false;
        }

        protected ISpecificationBase WithExceptionManagerBase(IExceptionManager exceptionManager)
        {
            ExceptionManager = exceptionManager.NotNull(nameof(exceptionManager));
            return this;
        }

        protected ISpecificationBase WithErrorBase(string errorMessage, int errorCode = ExceptionMessageTypes.DefaultErrorCode)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            return this;
        }

        protected ISpecificationBase WithExtendedDataBase(object extendedData)
        {
            ExtendedData = extendedData;
            return this;
        }
    }
}
