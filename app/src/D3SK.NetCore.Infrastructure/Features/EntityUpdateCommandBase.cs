using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Features;
using D3SK.NetCore.Infrastructure.Extensions;

namespace D3SK.NetCore.Infrastructure.Features
{
    public abstract class EntityUpdateCommandBase<TDomain, T, TStore, TCommandContainer>
        : EntityUpdateCommandBase<TDomain, T, int, TStore, TCommandContainer>
        where TDomain : IDomain
        where T : class, IEntity<int>
        where TStore : ITransactionStore
        where TCommandContainer : ICommandContainer<T, int, TStore>
    {
        protected EntityUpdateCommandBase(TCommandContainer commandContainer, IUpdateStrategy updateStrategy)
            : base(commandContainer, updateStrategy)
        {
        }
    }

    public abstract class EntityUpdateCommandBase<TDomain, T, TKey, TStore, TCommandContainer>
        : IEntityUpdateCommand<TDomain, T>
        where TDomain : IDomain
        where T : class, IEntity<TKey>
        where TStore : ITransactionStore
        where TCommandContainer : ICommandContainer<T, TKey, TStore>
    {
        protected readonly TCommandContainer CommandContainer;

        protected readonly IUpdateStrategy UpdateStrategy;

        public T CurrentItem { get; set; }

        public T OriginalItem { get; set; }

        protected EntityUpdateCommandBase(TCommandContainer commandContainer, IUpdateStrategy updateStrategy)
        {
            CommandContainer = commandContainer.NotNull(nameof(commandContainer));
            UpdateStrategy = updateStrategy.NotNull(nameof(updateStrategy));
        }

        public virtual async Task HandleAsync(IDomainInstance<TDomain> domainInstance)
        {
            var options = GetUpdateOptions();
            await CommandContainer.Store.InTransactionAsync(async transaction =>
            {
                var dbItem = await CommandContainer.FindAsync(CurrentItem.Id);
                await OnBeforeUpdateAsync(dbItem);
                await UpdateStrategy.UpdateEntityAsync(
                    CurrentItem,
                    OriginalItem,
                    dbItem,
                    onUpdateComplete: e => CommandContainer.UpdateAsync(CurrentItem, OriginalItem),
                    onPropertyChanged: OnPropertyChangedAsync,
                    options: options);
                await UpdateDependentsAsync(dbItem);
                await CommandContainer.Store.SaveChangesAsync();
                await OnAfterUpdateAsync();
            });
        }

        protected virtual UpdateEntityOptions GetUpdateOptions() => new UpdateEntityOptions();

        protected virtual Task UpdateDependentsAsync(T dbItem) => Task.CompletedTask;

        protected virtual Task OnPropertyChangedAsync(EntityPropertyUpdatedEventArgs<T> e) => Task.CompletedTask;

        protected virtual Task OnBeforeUpdateAsync(T dbItem) => Task.CompletedTask;

        protected virtual Task OnAfterUpdateAsync() => Task.CompletedTask;
    }
}