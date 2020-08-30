using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Specifications;

namespace D3SK.NetCore.Common.Extensions
{
    public static class SpecificationExtensions
    {
        public static ISpecification And(this ISpecification spec1, ISpecification spec2)
        {
            return new AndSpecification(spec1, spec2)
                .WithExceptionManager(spec1.ExceptionManager);
        }

        public static ISpecification Or(this ISpecification spec1, ISpecification spec2)
        {
            return new OrSpecification(spec1, spec2)
                .WithExceptionManager(spec1.ExceptionManager);
        }

        public static ISpecification Not(this ISpecification spec)
        {
            return new NotSpecification(spec).WithExceptionManager(spec.ExceptionManager);
        }

        public static ISpecification<T> And<T>(this ISpecification<T> spec1, ISpecification<T> spec2)
        {
            return new AndSpecification<T>(spec1, spec2)
                .WithExceptionManager(spec1.ExceptionManager);
        }

        public static ISpecification<T> Or<T>(this ISpecification<T> spec1, ISpecification<T> spec2)
        {
            return new OrSpecification<T>(spec1, spec2)
                .WithExceptionManager(spec1.ExceptionManager);
        }

        public static ISpecification<T> Not<T>(this ISpecification<T> spec)
        {
            return new NotSpecification<T>(spec).WithExceptionManager(spec.ExceptionManager);
        }

        public static IAsyncSpecification AndAsync(this IAsyncSpecification spec1, IAsyncSpecification spec2)
        {
            return new AsyncAndSpecification(spec1, spec2)
                .WithExceptionManager(spec1.ExceptionManager);
        }

        public static IAsyncSpecification Or(this IAsyncSpecification spec1, IAsyncSpecification spec2)
        {
            return new AsyncOrSpecification(spec1, spec2)
                .WithExceptionManager(spec1.ExceptionManager);
        }

        public static IAsyncSpecification Not(this IAsyncSpecification spec)
        {
            return new AsyncNotSpecification(spec).WithExceptionManager(spec.ExceptionManager);
        }

        public static IAsyncSpecification<T> And<T>(this IAsyncSpecification<T> spec1, IAsyncSpecification<T> spec2)
        {
            return new AsyncAndSpecification<T>(spec1, spec2)
                .WithExceptionManager(spec1.ExceptionManager);
        }

        public static IAsyncSpecification<T> Or<T>(this IAsyncSpecification<T> spec1, IAsyncSpecification<T> spec2)
        {
            return new AsyncOrSpecification<T>(spec1, spec2)
                .WithExceptionManager(spec1.ExceptionManager);
        }

        public static IAsyncSpecification<T> Not<T>(this IAsyncSpecification<T> spec)
        {
            return new AsyncNotSpecification<T>(spec).WithExceptionManager(spec.ExceptionManager);
        }
    }
}
