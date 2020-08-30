using D3SK.NetCore.Common.Models;
using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Common.Specifications
{
    public abstract class Specification : SpecificationBase, ISpecification
    {
        protected abstract bool IsSatisfiedCondition();

        public bool IsSatisfied() => HandleSpecification(IsSatisfiedCondition());

        public virtual ISpecification WithExceptionManager(IExceptionManager exceptionManager)
            => WithExceptionManagerBase(exceptionManager) as ISpecification;

        public virtual ISpecification WithError(string errorMessage, int errorCode = ExceptionMessageTypes.DefaultErrorCode)
            => WithErrorBase(errorMessage, errorCode) as ISpecification;

        public virtual ISpecification WithExtendedData(object extendedData)
            => WithExtendedDataBase(extendedData) as ISpecification;
    }

    public abstract class Specification<T> : SpecificationBase, ISpecification<T>
    {
        protected abstract bool IsSatisfiedByCondition(T item);

        public bool IsSatisfiedBy(T item) => HandleSpecification(IsSatisfiedByCondition(item));

        public virtual ISpecification<T> WithExceptionManager(IExceptionManager exceptionManager)
            => WithExceptionManagerBase(exceptionManager) as ISpecification<T>;

        public virtual ISpecification<T> WithError(string errorMessage, int errorCode = ExceptionMessageTypes.DefaultErrorCode)
            => WithErrorBase(errorMessage, errorCode) as ISpecification<T>;

        public virtual ISpecification<T> WithExtendedData(object extendedData)
            => WithExtendedDataBase(extendedData) as ISpecification<T>;
    }
}
