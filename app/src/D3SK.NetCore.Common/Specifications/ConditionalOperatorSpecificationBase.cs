﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Common.Specifications
{
    public abstract class ConditionalOperatorSpecificationBase : Specification, ISpecification
    {
        protected ISpecification Specification1 { get; }

        protected ISpecification Specification2 { get; }

        protected ConditionalOperatorSpecificationBase(ISpecification specification1, ISpecification specification2)
        {
            Specification1 = specification1.NotNull(nameof(specification1));
            Specification2 = specification2.NotNull(nameof(specification2));
        }
    }

    public abstract class ConditionalOperatorSpecificationBase<T> : Specification<T>, ISpecification<T>
    {
        protected ISpecification<T> Specification1 { get; }

        protected ISpecification<T> Specification2 { get; }

        protected ConditionalOperatorSpecificationBase(ISpecification<T> specification1,
            ISpecification<T> specification2)
        {
            Specification1 = specification1.NotNull(nameof(specification1));
            Specification2 = specification2.NotNull(nameof(specification2));
        }
    }

    public abstract class AsyncConditionalOperatorSpecificationBase : AsyncSpecification, IAsyncSpecification
    {
        protected IAsyncSpecification Specification1 { get; }

        protected IAsyncSpecification Specification2 { get; }

        protected AsyncConditionalOperatorSpecificationBase(IAsyncSpecification specification1,
            IAsyncSpecification specification2)
        {
            Specification1 = specification1.NotNull(nameof(specification1));
            Specification2 = specification2.NotNull(nameof(specification2));
        }
    }

    public abstract class AsyncConditionalOperatorSpecificationBase<T> : AsyncSpecification<T>, IAsyncSpecification<T>
    {
        protected IAsyncSpecification<T> Specification1 { get; }

        protected IAsyncSpecification<T> Specification2 { get; }

        protected AsyncConditionalOperatorSpecificationBase(IAsyncSpecification<T> specification1,
            IAsyncSpecification<T> specification2)
        {
            Specification1 = specification1.NotNull(nameof(specification1));
            Specification2 = specification2.NotNull(nameof(specification2));
        }
    }
}