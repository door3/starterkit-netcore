using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public interface IAsyncValidationEventHandler<in T> : IAsyncDomainEventHandler<IValidationEvent<T>>
    {
        Task<bool> IsValidAsync(T item);
    }
}
