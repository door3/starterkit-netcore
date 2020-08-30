using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Common.Specifications
{
    public abstract class ConditionalOperatorSpecificationBase : Specification
    {
        protected ISpecification Specification1 { get; }

        protected ISpecification Specification2 { get; }

        protected ConditionalOperatorSpecificationBase(ISpecification specification1, ISpecification specification2)
        {
            Specification1 = specification1.NotNull(nameof(specification1));
            Specification2 = specification2.NotNull(nameof(specification2));
        }

        public override ISpecification WithExceptionManager(IExceptionManager exceptionManager)
        {
            Specification1.WithExceptionManager(exceptionManager);
            Specification2.WithExceptionManager(exceptionManager);
            return base.WithExceptionManager(exceptionManager);
        }
    }

    public abstract class ConditionalOperatorSpecificationBase<T> : Specification<T>
    {
        protected ISpecification<T> Specification1 { get; }

        protected ISpecification<T> Specification2 { get; }

        protected ConditionalOperatorSpecificationBase(ISpecification<T> specification1,
            ISpecification<T> specification2)
        {
            Specification1 = specification1.NotNull(nameof(specification1));
            Specification2 = specification2.NotNull(nameof(specification2));
        }

        public override ISpecification<T> WithExceptionManager(IExceptionManager exceptionManager)
        {
            Specification1.WithExceptionManager(exceptionManager);
            Specification2.WithExceptionManager(exceptionManager);
            return base.WithExceptionManager(exceptionManager);
        }
    }

    public abstract class AsyncConditionalOperatorSpecificationBase : AsyncSpecification
    {
        protected IAsyncSpecification Specification1 { get; }

        protected IAsyncSpecification Specification2 { get; }

        protected AsyncConditionalOperatorSpecificationBase(IAsyncSpecification specification1,
            IAsyncSpecification specification2)
        {
            Specification1 = specification1.NotNull(nameof(specification1));
            Specification2 = specification2.NotNull(nameof(specification2));
        }

        public override IAsyncSpecification WithExceptionManager(IExceptionManager exceptionManager)
        {
            Specification1.WithExceptionManager(exceptionManager);
            Specification2.WithExceptionManager(exceptionManager);
            return base.WithExceptionManager(exceptionManager);
        }
    }

    public abstract class AsyncConditionalOperatorSpecificationBase<T> : AsyncSpecification<T>
    {
        protected IAsyncSpecification<T> Specification1 { get; }

        protected IAsyncSpecification<T> Specification2 { get; }

        protected AsyncConditionalOperatorSpecificationBase(IAsyncSpecification<T> specification1,
            IAsyncSpecification<T> specification2)
        {
            Specification1 = specification1.NotNull(nameof(specification1));
            Specification2 = specification2.NotNull(nameof(specification2));
        }

        public override IAsyncSpecification<T> WithExceptionManager(IExceptionManager exceptionManager)
        {
            Specification1.WithExceptionManager(exceptionManager);
            Specification2.WithExceptionManager(exceptionManager);
            return base.WithExceptionManager(exceptionManager);
        }
    }
}