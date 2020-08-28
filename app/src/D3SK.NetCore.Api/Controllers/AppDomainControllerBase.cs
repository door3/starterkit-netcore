using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain;
using Microsoft.AspNetCore.Mvc;

namespace D3SK.NetCore.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DomainControllerBase<TDomain> : ApiResponseControllerBase where TDomain : IDomain
    {
        protected IDomainInstance<TDomain> DomainInstance { get; }

        protected DomainControllerBase(IDomainInstance<TDomain> instance, IExceptionManager exceptionManager)
            : base(exceptionManager)
        {
            DomainInstance = instance.NotNull(nameof(instance));
        }
    }
}