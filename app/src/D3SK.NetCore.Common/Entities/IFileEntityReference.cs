using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public interface IFileEntityReference : IEntityReferenceBase
    {
        string Container { get; }

        string Path { get; }
    }
}
