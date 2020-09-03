using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Domain
{
    public interface IDomain
    {
        IDomainBus Bus { get; }

        IHandleDomainEventStrategy<IDomainEvent> EventStrategy { get; }

        IHandleDomainEventStrategy<IValidationEvent> ValidationStrategy { get; }

        void AddRole<TDomainRole>(TDomainRole role) where TDomainRole : IDomainRole;

        TDomainRole GetRole<TDomainRole>() where TDomainRole : IDomainRole;

        void HandlesEvent<THandler, TEvent>() 
            where THandler : class, IAsyncDomainEventHandler<TEvent>
            where TEvent : IDomainEvent;

        void HandlesValidation<THandler, T>() where THandler : class, IAsyncValidationEventHandler<T>;

        void HandlesBusEvent<THandler, TEvent>()
            where THandler : class, IAsyncDomainEventHandler<TEvent>
            where TEvent : IDomainBusEvent;
    }
}
