using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Domain.Stores;

namespace D3SK.NetCore.Domain.Features
{
    public interface IEntityDeleteCommand<TKey> : IStoreCommandFeature
    {
        TKey EntityId { get; set; }
    }

    public interface IEntityDeleteCommand<TDomain, T> : IEntityDeleteCommand<TDomain, T, int>
        where TDomain : IDomain
        where T : class, IEntity<int>
    {
    }

    public interface IEntityDeleteCommand<TDomain, T, TKey> : IAsyncCommandFeature<TDomain>, IEntityDeleteCommand<TKey>
        where TDomain : IDomain
        where T : class, IEntity<TKey>
    {
    }
}
