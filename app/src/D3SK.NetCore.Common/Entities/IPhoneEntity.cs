using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public interface IPhoneEntity : ICompositeEntity
    { 
        string Number { get; set; }
    }
}
