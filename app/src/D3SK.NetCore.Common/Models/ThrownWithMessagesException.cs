using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Models
{
    public class ThrownWithMessagesException : Exception
    {
        public IReadOnlyList<ExceptionMessage> Messages { get; }

        public object TempData { get; }

        public ThrownWithMessagesException(IReadOnlyList<ExceptionMessage> messages, object tempData = null)
        {
            Messages = messages;
            TempData = tempData;
        }
    }
}
