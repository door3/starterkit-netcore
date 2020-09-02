namespace D3SK.NetCore.Domain.Events
{
    public interface IValidationEvent : IDomainEventBase
    {
        bool IsValid { get; set; }
    }

    public interface IValidationEvent<out T> : IValidationEvent
    {
        T ObjectToValidate { get; }
    }
}
