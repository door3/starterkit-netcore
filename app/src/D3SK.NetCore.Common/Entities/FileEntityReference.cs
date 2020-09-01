using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Entities
{
    public class FileEntityReference : EntityReferenceBase, IFileEntityReference
    {
        public string Container { get; private set; }

        public string Path { get; private set; }

        public FileEntityReference()
        {
        }

        public FileEntityReference(string path, string name = null, string container = null, object extendedData = null)
            : base(name, extendedData)
        {
            Path = path;
            Container = container;
        }
    }
}
