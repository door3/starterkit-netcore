using D3SK.NetCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Infrastructure.Domain
{
    public abstract class DomainBase<TDomain> : DomainBase where TDomain : IDomain
    {
        protected DomainBase(
            IDomainBus bus,
            IQueryDomainRole<TDomain> queryRole,
            ICommandDomainRole<TDomain> commandRole,
            IHandleDomainEventStrategy<IDomainEvent> eventStrategy,
            IHandleDomainEventStrategy<IValidationEvent> validationStrategy)
            : base(bus, eventStrategy, validationStrategy)
        {
            AddRole(queryRole.NotNull(nameof(queryRole)));
            AddRole(commandRole.NotNull(nameof(commandRole)));
        }
    }

    public abstract class DomainBase : IDomain
    {
        protected IDictionary<Type, IDomainRole> Roles { get; } = new Dictionary<Type, IDomainRole>();

        public IDomainBus Bus { get; }

        public IHandleDomainEventStrategy<IDomainEvent> EventStrategy { get; }

        public IHandleDomainEventStrategy<IValidationEvent> ValidationStrategy { get; }

        protected DomainBase(
            IDomainBus bus,
            IHandleDomainEventStrategy<IDomainEvent> eventStrategy,
            IHandleDomainEventStrategy<IValidationEvent> validationStrategy)
        {
            Bus = bus.NotNull(nameof(bus));
            EventStrategy = eventStrategy.NotNull(nameof(eventStrategy));
            ValidationStrategy = validationStrategy.NotNull(nameof(validationStrategy));
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

        public void HandlesEvent<THandler, TEvent>() 
            where THandler : class, IAsyncDomainEventHandler<TEvent> 
            where TEvent : IDomainEvent
        {
            EventStrategy.AddAsyncHandler<THandler, TEvent>();
        }

        public void HandlesValidation<THandler, T>() where THandler : class, IAsyncValidationEventHandler<T>
        {
            ValidationStrategy.AddAsyncHandler<THandler, IValidationEvent<T>>();
        }

        public void HandlesBusEvent<THandler, TEvent>()
            where THandler : class, IAsyncDomainEventHandler<TEvent>
            where TEvent : IBusEvent
        {
            Bus.EventStrategy.AddAsyncHandler<THandler, TEvent>();
        }
    }
}
