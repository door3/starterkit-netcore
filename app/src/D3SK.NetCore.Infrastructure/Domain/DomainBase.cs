using D3SK.NetCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3SK.NetCore.Domain.Events;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Infrastructure.Domain
{
    public abstract class DomainBase : IDomain
    {
        protected IDictionary<Type, IDomainRole> Roles { get; } = new Dictionary<Type, IDomainRole>();

        public IHandleDomainMiddlewareStrategy<IDomainEvent> EventStrategy { get; }

        public IHandleDomainMiddlewareStrategy<IValidationEvent> ValidationStrategy { get; }

        protected DomainBase(
            IHandleDomainMiddlewareStrategy<IDomainEvent> eventStrategy,
            IHandleDomainMiddlewareStrategy<IValidationEvent> validationStrategy)
        {
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
    }
}
