using D3SK.NetCore.Common.Utilities;

namespace D3SK.NetCore.Common.Specifications
{
    public interface ISpecificationBase
    {
        IExceptionManager ExceptionManager { get; }

        string ErrorMessage { get; }

        int ErrorCode { get; }

        object ExtendedData { get; }
    }
}
