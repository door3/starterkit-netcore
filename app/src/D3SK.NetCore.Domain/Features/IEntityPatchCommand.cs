using System.Collections.Generic;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Domain.Features
{
    public interface IEntityPatchCommand<T> : IEntityUpdateCommand<T>
        where T : class, IEntityBase
    {
        IList<string> PropertiesToUpdate { get; set; }
    }

    public interface IEntityPatchCommand<TDomain, T> : IEntityUpdateCommand<TDomain, T>, IEntityPatchCommand<T>
        where TDomain : IDomain
        where T : class, IEntityBase
    {
    }
}
