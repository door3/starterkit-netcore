namespace D3SK.NetCore.Domain.Events
{
    public class DomainValidationEvent<T> : IValidationEvent<T>
    {
        public bool IsValid { get; }

        public T ObjectToValidate { get; }

        public DomainValidationEvent(T objectToValidate, bool isValid)
        {
            ObjectToValidate = objectToValidate;
            IsValid = isValid;
        }
    }
}
