using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Models;
using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Common.Specifications
{
    public interface IAsyncSpecification : ISpecificationBase
    {
        Task<bool> IsSatisfiedAsync();

        IAsyncSpecification WithExceptionManager(IExceptionManager exceptionManager);

        IAsyncSpecification WithError(string errorMessage, int errorCode = ExceptionMessageTypes.DefaultErrorCode);

        IAsyncSpecification WithExtendedData(object extendedData);
    }

    public interface IAsyncSpecification<in T> : ISpecificationBase
    {
        Task<bool> IsSatisfiedByAsync(T item);

        IAsyncSpecification<T> WithExceptionManager(IExceptionManager exceptionManager);

        IAsyncSpecification<T> WithError(string errorMessage, int errorCode = ExceptionMessageTypes.DefaultErrorCode);

        IAsyncSpecification<T> WithExtendedData(object extendedData);
    }
}
