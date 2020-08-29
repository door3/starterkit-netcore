using System;
using System.Threading;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;

namespace D3SK.NetCore.Infrastructure.Extensions
{
    public static class StoreExtensions
    {
        public static async Task<bool> InTransactionAsync(
            this ITransactionStore source,
            Func<IStoreTransaction, Task> actionAsync,
            CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            actionAsync.NotNull(nameof(actionAsync));

            var isExistingTransaction = source.IsInTransaction;
            var transaction = await source.BeginTransactionAsync(cancellationToken);

            await actionAsync(transaction);

            if (isExistingTransaction || transaction.IsComplete)
            {
                return false;
            }

            transaction.Commit();
            return true;
        }
    }
}
