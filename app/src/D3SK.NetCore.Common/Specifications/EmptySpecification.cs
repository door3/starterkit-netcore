using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Common.Specifications
{
    public class EmptySpecification : Specification
    {
        protected override bool IsSatisfiedCondition() => true;
    }

    public class EmptySpecification<T> : Specification<T>
    {
        protected override bool IsSatisfiedByCondition(T item) => true;
    }

    public class AsyncEmptySpecification : AsyncSpecification
    {
        protected override Task<bool> IsSatisfiedConditionAsync() => true.AsTask();
    }

    public class AsyncEmptySpecification<T> : AsyncSpecification<T>
    {
        protected override Task<bool> IsSatisfiedByConditionAsync(T item) => true.AsTask();
    }
}
