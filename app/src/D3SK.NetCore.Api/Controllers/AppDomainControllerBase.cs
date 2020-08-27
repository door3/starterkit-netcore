using System.Web.Http;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain;

namespace D3SK.NetCore.Api.Controllers
{
    public class AppDomainControllerBase<TDomain> : ApiResponseControllerBase where TDomain : IDomain
    {
        protected IDomainInstance<TDomain> DomainInstance { get; }

        protected AppDomainControllerBase(IDomainInstance<TDomain> instance, IExceptionManager exceptionManager)
            : base(exceptionManager)
        {
            DomainInstance = instance.NotNull(nameof(instance));
        }
    }
}