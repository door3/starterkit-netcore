namespace D3SK.NetCore.Domain.Events
{
    public class DomainValidationEvent<T> : IValidationEvent<T>
    {
        public bool IsValid { get; set; }

        public T ObjectToValidate { get; }

        public DomainValidationEvent(T objectToValidate)
        {
            ObjectToValidate = objectToValidate;
        }
    }
}
