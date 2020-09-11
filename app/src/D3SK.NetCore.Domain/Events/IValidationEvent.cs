namespace D3SK.NetCore.Domain.Events
{
    public interface IValidationEvent : IEventBase
    {
        bool IsValid { get; }
    }

    public interface IValidationEvent<out T> : IValidationEvent
    {
        T ObjectToValidate { get; }
    }
}
