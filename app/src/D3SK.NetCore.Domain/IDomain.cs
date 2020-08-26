using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Domain
{
    public interface IDomain
    {
        IHandleDomainMiddlewareStrategy<IDomainEvent> EventStrategy { get; }

        IHandleDomainMiddlewareStrategy<IValidationEvent> ValidationStrategy { get; }

        void AddRole<TDomainRole>(TDomainRole role) where TDomainRole : IDomainRole;

        TDomainRole GetRole<TDomainRole>() where TDomainRole : IDomainRole;
    }
}
