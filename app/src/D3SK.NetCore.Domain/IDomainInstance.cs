using System;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Domain
{
    public interface IDomainInstance
    {
        IServiceProvider ServiceProvider { get; }

        IExceptionManager ExceptionManager { get; }

        Task<Guid> PublishEventAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEventBase;

        Task<bool> ValidateAsync<T>(T item);

        Task<bool> ValidateAsync<T, TOptions>(T item, TOptions validationOptions);

        ICurrentUserManager GetCurrentUserManager();
    }

    public interface IDomainInstance<TDomain> : IDomainInstance
        where TDomain : IDomain
    {
        TDomain Domain { get; }

        Task<TResult> RunFeatureAsync<TDomainRole, TResult>(IAsyncQueryFeature<TDomain, TResult> feature)
            where TDomainRole : IQueryDomainRole<TDomain>;

        Task RunFeatureAsync<TDomainRole>(IAsyncCommandFeature<TDomain> feature)
            where TDomainRole : ICommandDomainRole<TDomain>;
    }
}
