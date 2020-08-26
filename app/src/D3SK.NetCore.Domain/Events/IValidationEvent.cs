namespace D3SK.NetCore.Domain.Events
{
    public interface IValidationEvent : IDomainMiddleware
    {
        object ObjectToValidate { get; }
    }
}
