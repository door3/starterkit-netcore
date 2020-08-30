using System.Threading.Tasks;

namespace D3SK.NetCore.Common.Specifications
{
    public class AndSpecification : ConditionalOperatorSpecificationBase
    {
        public AndSpecification(ISpecification specification1, ISpecification specification2)
            : base(specification1, specification2)
        {
        }

        protected override bool IsSatisfiedCondition() => Specification1.IsSatisfied() && Specification2.IsSatisfied();
    }

    public class AndSpecification<T> : ConditionalOperatorSpecificationBase<T>
    {
        public AndSpecification(ISpecification<T> specification1, ISpecification<T> specification2)
            : base(specification1, specification2)
        {
        }

        protected override bool IsSatisfiedByCondition(T item) =>
            Specification1.IsSatisfiedBy(item) && Specification2.IsSatisfiedBy(item);
    }

    public class AsyncAndSpecification : AsyncConditionalOperatorSpecificationBase
    {
        public AsyncAndSpecification(IAsyncSpecification specification1, IAsyncSpecification specification2)
            : base(specification1, specification2)
        {
        }

        protected override async Task<bool> IsSatisfiedConditionAsync() =>
            await Specification1.IsSatisfiedAsync() && await Specification2.IsSatisfiedAsync();
    }

    public class AsyncAndSpecification<T> : AsyncConditionalOperatorSpecificationBase<T>
    {
        public AsyncAndSpecification(IAsyncSpecification<T> specification1, IAsyncSpecification<T> specification2)
            : base(specification1, specification2)
        {
        }

        protected override async Task<bool> IsSatisfiedByConditionAsync(T item) =>
            await Specification1.IsSatisfiedByAsync(item) && await Specification2.IsSatisfiedByAsync(item);
    }
}