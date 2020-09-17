using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Domain.Events
{
    public class ValidationHandlerInfo : DomainEventHandlerInfo
    {
        public Type ValidationOptionsType { get; }

        public ValidationHandlerInfo(Type objectType, Type handlerType, Type validationOptionsType, HandleDomainEventOptions options) 
            : base(objectType, handlerType, options)
        {
            ValidationOptionsType = validationOptionsType;
        }
    }
}
