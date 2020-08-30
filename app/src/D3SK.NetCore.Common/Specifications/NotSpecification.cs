using System.Threading.Tasks;

namespace D3SK.NetCore.Common.Specifications
{
    public class NotSpecification : UnaryOperatorSpecificationBase
    {
        public NotSpecification(ISpecification specification) : base(specification)
        {
        }

        protected override bool IsSatisfiedCondition() => !Specification.IsSatisfied();
    }

    public class NotSpecification<T> : UnaryOperatorSpecificationBase<T>
    {
        public NotSpecification(ISpecification<T> specification) : base(specification)
        {
        }

        protected override bool IsSatisfiedByCondition(T item) => !Specification.IsSatisfiedBy(item);
    }

    public class AsyncNotSpecification : AsyncUnaryOperatorSpecificationBase
    {
        public AsyncNotSpecification(IAsyncSpecification specification) : base(specification)
        {
        }

        protected override async Task<bool> IsSatisfiedConditionAsync() => !await Specification.IsSatisfiedAsync();
    }

    public class AsyncNotSpecification<T> : AsyncUnaryOperatorSpecificationBase<T>
    {
        public AsyncNotSpecification(IAsyncSpecification<T> specification) : base(specification)
        {
        }

        protected override async Task<bool> IsSatisfiedByConditionAsync(T item) =>
            !await Specification.IsSatisfiedByAsync(item);
    }
}