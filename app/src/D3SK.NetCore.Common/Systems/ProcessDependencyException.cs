using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Systems
{
    public class ProcessDependencyException : Exception
    {
        public ProcessDependencyException()
        {
        }

        public ProcessDependencyException(string message) : base(message)
        {
        }
    }
}
