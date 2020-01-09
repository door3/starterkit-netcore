using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public interface IAuditEntity : IEntityBase
    {
        DateTimeOffset CreatedDate { get; }

        string CreatedByUser { get; }

        DateTimeOffset LastModifiedDate { get; }

        string LastModifiedByUser { get; }

        void OnAdded(DateTimeOffset createdDate, string createdByUser);

        void OnUpdated(DateTimeOffset lastModifiedDate, string lastModifiedByUser);
    }
}
