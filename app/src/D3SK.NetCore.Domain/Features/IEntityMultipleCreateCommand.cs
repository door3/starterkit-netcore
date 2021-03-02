using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Domain.Stores;

namespace D3SK.NetCore.Domain.Features
{
    public interface IEntityMultipleCreateCommand<T> : IStoreCommandFeature where T : class, IEntityBase
    {
        T[] Items { get; set; }
    }

    public interface IEntityMultipleCreateCommand<TDomain, T> : IAsyncCommandFeature<TDomain>, IEntityMultipleCreateCommand<T>
        where TDomain : IDomain
        where T : class, IEntityBase
    {
    }
}
