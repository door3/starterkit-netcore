using System;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Common.Specifications
{
    public class FuncSpecification : Specification
    {
        protected Func<bool> Condition { get; }

        public FuncSpecification(Func<bool> condition)
        {
            Condition = condition.NotNull(nameof(condition));
        }

        protected override bool IsSatisfiedCondition() => Condition();
    }

    public class FuncSpecification<T> : Specification<T>
    {
        protected Func<T, bool> Condition { get; }

        public FuncSpecification(Func<T, bool> condition)
        {
            Condition = condition.NotNull(nameof(condition));
        }

        protected override bool IsSatisfiedByCondition(T item) => Condition(item);
    }

    public class AsyncFuncSpecification : AsyncSpecification
    {
        protected Func<Task<bool>> ConditionAsync { get; }

        public AsyncFuncSpecification(Func<Task<bool>> conditionAsync)
        {
            ConditionAsync = conditionAsync.NotNull(nameof(conditionAsync));
        }

        protected override Task<bool> IsSatisfiedConditionAsync() => ConditionAsync();
    }

    public class AsyncFuncSpecification<T> : AsyncSpecification<T>
    {
        protected Func<T, Task<bool>> ConditionAsync { get; }

        public AsyncFuncSpecification(Func<T, Task<bool>> conditionAsync)
        {
            ConditionAsync = conditionAsync.NotNull(nameof(conditionAsync));
        }

        protected override Task<bool> IsSatisfiedByConditionAsync(T item) => ConditionAsync(item);
    }
}
