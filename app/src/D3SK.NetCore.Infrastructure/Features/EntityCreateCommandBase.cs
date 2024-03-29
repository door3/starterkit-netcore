﻿using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Features;
using D3SK.NetCore.Infrastructure.Extensions;

namespace D3SK.NetCore.Infrastructure.Features
{
    public abstract class EntityCreateCommandBase<TDomain, T, TStore, TCommandContainer>
        : IEntityCreateCommand<TDomain, T>
        where TDomain : IDomain
        where T : class, IEntityBase
        where TStore : ITransactionStore
        where TCommandContainer : ICommandContainerBase<T, TStore>
    {
        protected readonly TCommandContainer CommandContainer;

        protected readonly IUpdateStrategy UpdateStrategy;

        public T CurrentItem { get; set; }

        protected EntityCreateCommandBase(TCommandContainer commandContainer, IUpdateStrategy updateStrategy)
        {
            CommandContainer = commandContainer.NotNull(nameof(commandContainer));
            UpdateStrategy = updateStrategy.NotNull(nameof(updateStrategy));
        }

        public virtual async Task HandleAsync(IDomainInstance<TDomain> domainInstance)
        {
            await CommandContainer.Store.InTransactionAsync(async transaction =>
            {
                await OnBeforeCreateAsync(domainInstance);
                await UpdateStrategy.AddEntityAsync(CurrentItem, e => CommandContainer.AddAsync(e.Entity));
                await OnBeforeSaveChangesAsync(domainInstance);
                await CommandContainer.Store.SaveChangesAsync();
                await OnAfterCreateAsync(domainInstance);
            });
            await OnTransactionCompleteAsync(domainInstance);
        }

        protected virtual Task OnBeforeCreateAsync(IDomainInstance<TDomain> domainInstance) => Task.CompletedTask;

        protected virtual Task OnBeforeSaveChangesAsync(IDomainInstance<TDomain> domainInstance) => Task.CompletedTask;

        protected virtual Task OnAfterCreateAsync(IDomainInstance<TDomain> domainInstance) => Task.CompletedTask;

        protected virtual Task OnTransactionCompleteAsync(IDomainInstance<TDomain> domainInstance) => Task.CompletedTask;
    }
}
