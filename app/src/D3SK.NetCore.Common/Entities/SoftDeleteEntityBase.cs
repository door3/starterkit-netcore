using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public class SoftDeleteEntityBase : SoftDeleteEntityBase<int>
    {
        public SoftDeleteEntityBase()
        {
        }

        public SoftDeleteEntityBase(int id) : base(id)
        {
        }
    }

    public class SoftDeleteEntityBase<TKey> : EntityBase<TKey>, ISoftDeleteEntity
    {
        public bool IsDeleted { get; set; }

        public SoftDeleteEntityBase()
        {
        }

        public SoftDeleteEntityBase(TKey id) : base(id)
        {
        }

        public void SetDeleted(bool isDeleted = true)
        {
            IsDeleted = isDeleted;
        }
    }
}
