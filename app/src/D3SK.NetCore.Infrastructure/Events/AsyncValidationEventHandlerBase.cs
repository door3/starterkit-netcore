using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Domain.Events;

namespace D3SK.NetCore.Infrastructure.Events
{
    public abstract class AsyncValidationEventHandlerBase<T> : AsyncDomainEventHandlerBase<IValidationEvent<T>>, 
        IAsyncValidationEventHandler<T>
    {
        public override async Task HandleAsync(IValidationEvent<T> validationEvent)
        {
            if (await IsValidAsync(validationEvent.ObjectToValidate))
            {
                validationEvent.IsValid = true;
            }
        }

        public abstract Task<bool> IsValidAsync(T item);
    }
}
