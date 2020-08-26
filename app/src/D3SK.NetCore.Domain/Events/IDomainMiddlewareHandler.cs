using System.Threading.Tasks;

namespace D3SK.NetCore.Domain.Events
{
    public interface IAsyncDomainMiddlewareHandler<in TMiddleware>  where TMiddleware : IDomainMiddleware
    {
        Task HandleAsync(TMiddleware context);
    }
}
