using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
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
