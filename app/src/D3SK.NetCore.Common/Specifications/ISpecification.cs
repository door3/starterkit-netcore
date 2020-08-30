using D3SK.NetCore.Common.Models;
using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Common.Specifications
{
    public interface ISpecification : ISpecificationBase
    {
        bool IsSatisfied();

        ISpecification WithExceptionManager(IExceptionManager exceptionManager);

        ISpecification WithError(string errorMessage, int errorCode = ExceptionMessageTypes.DefaultErrorCode);

        ISpecification WithExtendedData(object extendedData);
    }

    public interface ISpecification<in T> : ISpecificationBase
    {
        bool IsSatisfiedBy(T item);

        ISpecification<T> WithExceptionManager(IExceptionManager exceptionManager);

        ISpecification<T> WithError(string errorMessage, int errorCode = ExceptionMessageTypes.DefaultErrorCode);

        ISpecification<T> WithExtendedData(object extendedData);
    }
}
