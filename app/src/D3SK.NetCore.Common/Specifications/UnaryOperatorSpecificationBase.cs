using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Common.Specifications
{
    public abstract class UnaryOperatorSpecificationBase : Specification, ISpecification
    {
        protected ISpecification Specification { get; }

        protected UnaryOperatorSpecificationBase(ISpecification specification)
        {
            Specification = specification.NotNull(nameof(specification));
        }
    }

    public abstract class UnaryOperatorSpecificationBase<T> : Specification<T>, ISpecification<T>
    {
        protected ISpecification<T> Specification { get; }

        protected UnaryOperatorSpecificationBase(ISpecification<T> specification)
        {
            Specification = specification.NotNull(nameof(specification));
        }
    }

    public abstract class AsyncUnaryOperatorSpecificationBase : AsyncSpecification, IAsyncSpecification
    {
        protected IAsyncSpecification Specification { get; }

        protected AsyncUnaryOperatorSpecificationBase(IAsyncSpecification specification)
        {
            Specification = specification.NotNull(nameof(specification));
        }
    }

    public abstract class AsyncUnaryOperatorSpecificationBase<T> : AsyncSpecification<T>, IAsyncSpecification<T>
    {
        protected IAsyncSpecification<T> Specification { get; }

        protected AsyncUnaryOperatorSpecificationBase(IAsyncSpecification<T> specification)
        {
            Specification = specification.NotNull(nameof(specification));
        }
    }
}
