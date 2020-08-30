using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Common.Specifications
{
    public abstract class UnaryOperatorSpecificationBase : Specification
    {
        protected ISpecification Specification { get; }

        protected UnaryOperatorSpecificationBase(ISpecification specification)
        {
            Specification = specification.NotNull(nameof(specification));
        }

        public override ISpecification WithExceptionManager(IExceptionManager exceptionManager)
        {
            Specification.WithExceptionManager(exceptionManager);
            return base.WithExceptionManager(exceptionManager);
        }
    }

    public abstract class UnaryOperatorSpecificationBase<T> : Specification<T>
    {
        protected ISpecification<T> Specification { get; }

        protected UnaryOperatorSpecificationBase(ISpecification<T> specification)
        {
            Specification = specification.NotNull(nameof(specification));
        }

        public override ISpecification<T> WithExceptionManager(IExceptionManager exceptionManager)
        {
            Specification.WithExceptionManager(exceptionManager);
            return base.WithExceptionManager(exceptionManager);
        }
    }

    public abstract class AsyncUnaryOperatorSpecificationBase : AsyncSpecification
    {
        protected IAsyncSpecification Specification { get; }

        protected AsyncUnaryOperatorSpecificationBase(IAsyncSpecification specification)
        {
            Specification = specification.NotNull(nameof(specification));
        }

        public override IAsyncSpecification WithExceptionManager(IExceptionManager exceptionManager)
        {
            Specification.WithExceptionManager(exceptionManager);
            return base.WithExceptionManager(exceptionManager);
        }
    }

    public abstract class AsyncUnaryOperatorSpecificationBase<T> : AsyncSpecification<T>
    {
        protected IAsyncSpecification<T> Specification { get; }

        protected AsyncUnaryOperatorSpecificationBase(IAsyncSpecification<T> specification)
        {
            Specification = specification.NotNull(nameof(specification));
        }

        public override IAsyncSpecification<T> WithExceptionManager(IExceptionManager exceptionManager)
        {
            Specification.WithExceptionManager(exceptionManager);
            return base.WithExceptionManager(exceptionManager);
        }
    }
}
