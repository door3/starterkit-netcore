using System.Collections.Generic;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Domain.Features
{
    public interface IEntityMultiplePatchCommand<T> : IEntityMultipleUpdateCommand<T>
        where T : class, IEntityBase
    {
        IList<string> PropertiesToUpdate { get; set; }
    }

    public interface IEntityMultiplePatchCommand<TDomain, T> : IEntityMultipleUpdateCommand<TDomain, T>, IEntityMultiplePatchCommand<T>
        where TDomain : IDomain
        where T : class, IEntityBase
    {
    }
}
