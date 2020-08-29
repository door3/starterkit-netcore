using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Features;
using D3SK.NetCore.Infrastructure.Extensions;

namespace D3SK.NetCore.Infrastructure.Features
{
    public abstract class EntityDeleteCommandBase<TDomain, T, TStore, TCommandContainer>
        : EntityDeleteCommandBase<TDomain, T, int, TStore, TCommandContainer>
        where TDomain : IDomain
        where T : class, IEntity<int>
        where TStore : ITransactionStore
        where TCommandContainer : ICommandContainer<T, int, TStore>
    {
        protected EntityDeleteCommandBase(TCommandContainer commandContainer, IUpdateStrategy updateStrategy)
            : base(commandContainer, updateStrategy)
        {
        }
    }

    public abstract class EntityDeleteCommandBase<TDomain, T, TKey, TStore, TCommandContainer>
        : IEntityDeleteCommand<TDomain, T, TKey>
        where TDomain : IDomain
        where T : class, IEntity<TKey>
        where TStore : ITransactionStore
        where TCommandContainer : ICommandContainer<T, TKey, TStore>
    {
        protected readonly TCommandContainer CommandContainer;

        protected readonly IUpdateStrategy UpdateStrategy;

        public TKey EntityId { get; set; }

        protected EntityDeleteCommandBase(TCommandContainer commandContainer, IUpdateStrategy updateStrategy)
        {
            CommandContainer = commandContainer.NotNull(nameof(commandContainer));
            UpdateStrategy = updateStrategy.NotNull(nameof(updateStrategy));
        }

        public virtual async Task HandleAsync(IDomainInstance<TDomain> domainInstance)
        {
            await CommandContainer.Store.InTransactionAsync(async transaction =>
            {
                var dbItem = await CommandContainer.FindAsync(EntityId);
                await OnBeforeDeleteAsync(dbItem);
                await UpdateStrategy.DeleteEntityAsync(dbItem, e => CommandContainer.DeleteAsync(EntityId));
                await CommandContainer.Store.SaveChangesAsync();
                await OnAfterDeleteAsync();
            });
        }

        protected virtual Task OnBeforeDeleteAsync(T dbItem) => Task.CompletedTask;

        protected virtual Task OnAfterDeleteAsync() => Task.CompletedTask;
    }
}