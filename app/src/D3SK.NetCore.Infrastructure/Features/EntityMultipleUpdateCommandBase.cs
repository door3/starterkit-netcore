using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Features;
using D3SK.NetCore.Infrastructure.Extensions;

namespace D3SK.NetCore.Infrastructure.Features
{
    public abstract class EntityMultipleUpdateCommandBase<TDomain, T, TStore, TCommandContainer>
        : EntityMultipleUpdateCommandBase<TDomain, T, int, TStore, TCommandContainer>
        where TDomain : IDomain
        where T : class, IEntity<int>
        where TStore : ITransactionStore
        where TCommandContainer : ICommandContainer<T, int, TStore>
    {
        protected EntityMultipleUpdateCommandBase(TCommandContainer commandContainer, IUpdateStrategy updateStrategy)
            : base(commandContainer, updateStrategy)
        {
        }
    }

    public abstract class EntityMultipleUpdateCommandBase<TDomain, T, TKey, TStore, TCommandContainer>
        : IEntityMultipleUpdateCommand<TDomain, T>
        where TDomain : IDomain
        where T : class, IEntity<TKey>
        where TStore : ITransactionStore
        where TCommandContainer : ICommandContainer<T, TKey, TStore>
    {
        protected readonly TCommandContainer CommandContainer;

        protected readonly IUpdateStrategy UpdateStrategy;

        public Item<T>[] Items { get; set; }

        protected EntityMultipleUpdateCommandBase(TCommandContainer commandContainer, IUpdateStrategy updateStrategy)
        {
            CommandContainer = commandContainer.NotNull(nameof(commandContainer));
            UpdateStrategy = updateStrategy.NotNull(nameof(updateStrategy));
        }

        public virtual async Task HandleAsync(IDomainInstance<TDomain> domainInstance)
        {
            var options = GetUpdateOptions();
            await CommandContainer.Store.InTransactionAsync(async transaction =>
            {
                foreach (var item in Items)
                {
                    var dbStoreItem = await CommandContainer.FindAsync(item.CurrentItem.Id);
                    await OnBeforeUpdateAsync(dbStoreItem);
                    await UpdateStrategy.UpdateEntityAsync(
                        item.CurrentItem,
                        item.OriginalItem,
                        dbStoreItem,
                        onUpdateComplete: e => CommandContainer.UpdateAsync(item.CurrentItem, item.OriginalItem),
                        onPropertyChanged: OnPropertyChangedAsync,
                        options: options);
                    await UpdateDependentsAsync(item.CurrentItem, item.OriginalItem, dbStoreItem);
                }

                await OnBeforeSaveAsync();
                await CommandContainer.Store.SaveChangesAsync();
                await OnAfterUpdateAsync();
            });
        }

        protected virtual UpdateEntityOptions GetUpdateOptions() => new UpdateEntityOptions();

        protected virtual Task UpdateDependentsAsync(T currentItem, T originalItem, T dbStoreItem) => Task.CompletedTask;

        protected virtual Task OnPropertyChangedAsync(EntityPropertyUpdatedEventArgs<T> e) => Task.CompletedTask;

        protected virtual Task OnBeforeUpdateAsync(T dbStoreItem) => Task.CompletedTask;

        protected virtual Task OnBeforeSaveAsync() => Task.CompletedTask;

        protected virtual Task OnAfterUpdateAsync() => Task.CompletedTask;
    }
}
