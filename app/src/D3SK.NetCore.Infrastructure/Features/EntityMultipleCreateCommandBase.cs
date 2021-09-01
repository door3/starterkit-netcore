using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Features;
using D3SK.NetCore.Infrastructure.Extensions;

namespace D3SK.NetCore.Infrastructure.Features
{
    public abstract class EntityMultipleCreateCommandBase<TDomain, T, TStore, TCommandContainer> : EntityMultipleCreateCommandBase<TDomain, T, int, TStore, TCommandContainer> where TDomain : IDomain where T : class, IEntity<int> where TStore : ITransactionStore where TCommandContainer : ICommandContainer<T, int, TStore>
    {
        protected EntityMultipleCreateCommandBase(TCommandContainer commandContainer, IUpdateStrategy updateStrategy) : base(commandContainer, updateStrategy)
        {
        }
    }

    public abstract class EntityMultipleCreateCommandBase<TDomain, T, TKey, TStore, TCommandContainer> : IEntityMultipleCreateCommand<TDomain, T> where TDomain : IDomain where T : class, IEntity<TKey> where TStore : ITransactionStore where TCommandContainer : ICommandContainer<T, TKey, TStore>
    {
        protected readonly TCommandContainer CommandContainer;

        protected readonly IUpdateStrategy UpdateStrategy;

        protected EntityMultipleCreateCommandBase(TCommandContainer commandContainer, IUpdateStrategy updateStrategy)
        {
            CommandContainer = commandContainer.NotNull(nameof(commandContainer));
            UpdateStrategy = updateStrategy.NotNull(nameof(updateStrategy));
        }

        public T[] Items { get; set; }

        public virtual async Task HandleAsync(IDomainInstance<TDomain> domainInstance)
        {
            await CommandContainer.Store.InTransactionAsync(async transaction =>
            {
                foreach (var item in Items)
                {
                    await OnBeforeCreateAsync();
                    await UpdateStrategy.AddEntityAsync(item, e => CommandContainer.AddAsync(e.Entity));
                    await OnAfterCreateAsync();
                }

                await OnBeforeSaveChangesAsync();
                await CommandContainer.Store.SaveChangesAsync();
                await OnAfterSaveChangesAsync();
            });

            await OnTransactionCompleteAsync(domainInstance);
        }

        protected virtual Task OnBeforeCreateAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnAfterCreateAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnBeforeSaveChangesAsync()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnAfterSaveChangesAsync() => Task.CompletedTask;

        protected virtual Task OnTransactionCompleteAsync(IDomainInstance<TDomain> domainInstance) => Task.CompletedTask;
    }
}
