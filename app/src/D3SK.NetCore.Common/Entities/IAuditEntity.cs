using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public interface IAuditEntityBase : IEntityBase
    {
        DateTimeOffset CreatedDate { get; }

        DateTimeOffset LastModifiedDate { get; }

        void OnAdded(DateTimeOffset createdDate, object createdByUser);

        void OnUpdated(DateTimeOffset lastModifiedDate, object lastModifiedByUser);
    }

    public interface IAuditEntity : IAuditEntity<int?>
    {
    }

    public interface IAuditEntity<TUserKey> : IAuditEntityBase
    {
        TUserKey CreatedByUser { get; }
        
        TUserKey LastModifiedByUser { get; }

        void OnAdded(DateTimeOffset createdDate, TUserKey createdByUser);

        void OnUpdated(DateTimeOffset lastModifiedDate, TUserKey lastModifiedByUser);
    }
}
