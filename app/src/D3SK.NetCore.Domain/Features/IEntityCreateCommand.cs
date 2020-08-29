using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Domain.Stores;

namespace D3SK.NetCore.Domain.Features
{
    public interface IEntityCreateCommand<T> : IStoreCommandFeature
        where T : class, IEntityBase
    {
        T CurrentItem { get; set; }
    }

    public interface IEntityCreateCommand<TDomain, T> : IAsyncCommandFeature<TDomain>, IEntityCreateCommand<T>
        where TDomain : IDomain
        where T : class, IEntityBase
    {
    }
}
