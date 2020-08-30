using System.Threading.Tasks;
using D3SK.NetCore.Common.Models;
using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Common.Specifications
{
    public abstract class AsyncSpecification : SpecificationBase, IAsyncSpecification
    {
        protected abstract Task<bool> IsSatisfiedConditionAsync();

        public async Task<bool> IsSatisfiedAsync() => HandleSpecification(await IsSatisfiedConditionAsync());

        public virtual IAsyncSpecification WithExceptionManager(IExceptionManager exceptionManager)
            => WithExceptionManagerBase(exceptionManager) as IAsyncSpecification;

        public virtual IAsyncSpecification WithError(string errorMessage, int errorCode = ExceptionMessageTypes.DefaultErrorCode)
            => WithErrorBase(errorMessage, errorCode) as IAsyncSpecification;

        public virtual IAsyncSpecification WithExtendedData(object extendedData)
            => WithExtendedDataBase(extendedData) as IAsyncSpecification;
    }

    public abstract class AsyncSpecification<T> : SpecificationBase, IAsyncSpecification<T>
    {
        protected abstract Task<bool> IsSatisfiedByConditionAsync(T item);

        public async Task<bool> IsSatisfiedByAsync(T item) => HandleSpecification(await IsSatisfiedByConditionAsync(item));

        public virtual IAsyncSpecification<T> WithExceptionManager(IExceptionManager exceptionManager)
            => WithExceptionManagerBase(exceptionManager) as IAsyncSpecification<T>;

        public virtual IAsyncSpecification<T> WithError(string errorMessage, int errorCode = ExceptionMessageTypes.DefaultErrorCode)
            => WithErrorBase(errorMessage, errorCode) as IAsyncSpecification<T>;

        public virtual IAsyncSpecification<T> WithExtendedData(object extendedData)
            => WithExtendedDataBase(extendedData) as IAsyncSpecification<T>;
    }
}
