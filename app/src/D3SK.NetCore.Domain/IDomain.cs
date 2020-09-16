using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Domain
{
    public interface IDomain
    {
        IDomainBus Bus { get; }

        void AddRole<TDomainRole>(TDomainRole role) where TDomainRole : IDomainRole;

        TDomainRole GetRole<TDomainRole>() where TDomainRole : IDomainRole;
    }

    public interface IDomain<TDomain> : IDomain where TDomain : IDomain
    {
        IHandleDomainEventStrategy<IDomainEvent, TDomain> EventStrategy { get; }

        IHandleValidationStrategy<TDomain> ValidationStrategy { get; }

        void HandlesEvent<THandler, TEvent>()
            where THandler : class, IAsyncDomainEventHandler<TEvent, TDomain>
            where TEvent : IDomainEvent;

        void HandlesValidation<THandler, T>() where THandler : class, IAsyncValidator<T, TDomain>;

        void HandlesValidation<THandler, T, TOptions>() where THandler : class, IAsyncValidator<T, TOptions, TDomain>;

        void HandlesBusEvent<THandler, TEvent>()
            where THandler : class, IAsyncBusEventHandler<TEvent>
            where TEvent : IBusEvent;
    }

}
