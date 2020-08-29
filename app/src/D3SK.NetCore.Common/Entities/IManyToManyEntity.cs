using System.Collections.Generic;

namespace D3SK.NetCore.Common.Entities
{
    public interface IManyToManyEntity : IEntityBase
    {
        IEnumerable<object> GetUniqueValues();
    }
}
