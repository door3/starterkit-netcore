using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Queries;

namespace D3SK.NetCore.Domain.Features
{
    public interface IEntityCountQuery<T> : IFilterable
    {
    }

    public interface IEntityCountQuery<TDomain, T> : IAsyncQueryFeature<TDomain, int>, IEntityCountQuery<T>
        where TDomain : IDomain
        where T : class, IEntityBase
    {
    }
}
