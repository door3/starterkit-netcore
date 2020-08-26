using System.Collections.Generic;

namespace D3SK.NetCore.Common.Queries
{
    public interface IProjection
    {
        IList<string> SelectProperties { get; set; }
    }
}
