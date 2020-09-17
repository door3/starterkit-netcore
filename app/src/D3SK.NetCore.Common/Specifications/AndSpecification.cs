using System.Threading.Tasks;

namespace D3SK.NetCore.Common.Specifications
{
    public class AndSpecification : ConditionalOperatorSpecificationBase
    {
        public AndSpecification(ISpecification specification1, ISpecification specification2)
            : base(specification1, specification2)
        {
        }

        protected override bool IsSatisfiedCondition()
        {
            var isSpec1Satisfied = Specification1.IsSatisfied();
            var isSpec2Satisfied = Specification2.IsSatisfied();

            return isSpec1Satisfied && isSpec2Satisfied;
        }
    }

    public class AndSpecification<T> : ConditionalOperatorSpecificationBase<T>
    {
        public AndSpecification(ISpecification<T> specification1, ISpecification<T> specification2)
            : base(specification1, specification2)
        {
        }

        protected override bool IsSatisfiedByCondition(T item)
        {
            var isSpec1Satisfied = Specification1.IsSatisfiedBy(item);
            var isSpec2Satisfied = Specification2.IsSatisfiedBy(item);

            return isSpec1Satisfied && isSpec2Satisfied;
        }
    }

    public class AsyncAndSpecification : AsyncConditionalOperatorSpecificationBase
    {
        public AsyncAndSpecification(IAsyncSpecification specification1, IAsyncSpecification specification2)
            : base(specification1, specification2)
        {
        }

        protected override async Task<bool> IsSatisfiedConditionAsync()
        {
            var isSpec1Satisfied = await Specification1.IsSatisfiedAsync();
            var isSpec2Satisfied = await Specification2.IsSatisfiedAsync();

            return isSpec1Satisfied && isSpec2Satisfied;
        }
    }

    public class AsyncAndSpecification<T> : AsyncConditionalOperatorSpecificationBase<T>
    {
        public AsyncAndSpecification(IAsyncSpecification<T> specification1, IAsyncSpecification<T> specification2)
            : base(specification1, specification2)
        {
        }

        protected override async Task<bool> IsSatisfiedByConditionAsync(T item)
        {
            var isSpec1Satisfied = await Specification1.IsSatisfiedByAsync(item);
            var isSpec2Satisfied = await Specification2.IsSatisfiedByAsync(item);

            return isSpec1Satisfied && isSpec2Satisfied;
        }
}
}