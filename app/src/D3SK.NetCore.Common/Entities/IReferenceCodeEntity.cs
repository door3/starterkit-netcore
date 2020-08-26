using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public interface IReferenceCodeEntity : IReferenceCodeEntity<string>
    {
    }

    public interface IReferenceCodeEntity<TCodeType> : IEntityBase
    {
        TCodeType ReferenceCode { get; set; }
    }
}
