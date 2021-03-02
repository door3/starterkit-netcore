using D3SK.NetCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Domain.Models;
using Microsoft.Extensions.Options;

namespace D3SK.NetCore.Infrastructure.Domain
{
    public abstract class DomainBase<TDomain> : DomainBase, IDomain<TDomain> where TDomain : IDomain
    {
        public IHandleDomainEventStrategy<IDomainEvent, TDomain> EventStrategy { get; }

        public IHandleValidationStrategy<TDomain> ValidationStrategy { get; }

        protected DomainBase(
            IOptions<DomainOptions> domainOptions,
            IDomainBus bus,
            IQueryDomainRole<TDomain> queryRole,
            ICommandDomainRole<TDomain> commandRole,
            IHandleDomainEventStrategy<IDomainEvent, TDomain> eventStrategy,
            IHandleValidationStrategy<TDomain> validationStrategy)
            : base(domainOptions, bus)
        {
            AddRole(queryRole.NotNull(nameof(queryRole)));
            AddRole(commandRole.NotNull(nameof(commandRole)));
            EventStrategy = eventStrategy.NotNull(nameof(eventStrategy));
            ValidationStrategy = validationStrategy.NotNull(nameof(validationStrategy));
        }

        public void HandlesEvent<THandler, TEvent>()
            where THandler : class, IAsyncDomainEventHandler<TEvent, TDomain>
            where TEvent : IDomainEvent
        {
            EventStrategy.AddAsyncHandler<THandler, TEvent>();
        }

        public void HandlesValidation<THandler, T>() where THandler : class, IAsyncValidator<T, TDomain>
        {
            ValidationStrategy.AddAsyncHandler<THandler, T>();
        }

        public void HandlesValidation<THandler, T, TOptions>() where THandler : class, IAsyncValidator<T, TOptions, TDomain>
        {
            ValidationStrategy.AddAsyncHandler<THandler, T, TOptions>();
        }

        public void HandlesBusEvent<THandler, TEvent>()
            where THandler : class, IAsyncBusEventHandler<TEvent>
            where TEvent : IBusEvent
        {
            Bus.EventStrategy.AddAsyncHandler<THandler, TEvent>();
        }
    }

    public abstract class DomainBase : IDomain
    {
        public DomainOptions DomainOptions { get; }

        protected IDictionary<Type, IDomainRole> Roles { get; } = new Dictionary<Type, IDomainRole>();

        public IDomainBus Bus { get; }

        protected DomainBase(IOptions<DomainOptions> domainOptions, IDomainBus bus)
        {
            DomainOptions = domainOptions.NotNull(nameof(domainOptions)).Value;
            Bus = bus.NotNull(nameof(bus));
        }

        protected IDomainRole GetRole(Type roleType)
        {
            return Roles.SingleOrDefault(r => r.Key == roleType).Value;
        }

        public void AddRole<TDomainRole>(TDomainRole role) where TDomainRole : IDomainRole
        {
            role.NotNull(nameof(role));
            Roles.Add(typeof(TDomainRole), role);
        }

        public TDomainRole GetRole<TDomainRole>() where TDomainRole : IDomainRole
        {
            return (TDomainRole) GetRole(typeof(TDomainRole));
        }
    }
}
