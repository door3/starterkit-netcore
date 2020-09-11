using System.Linq;
using System.Net;
using System.Threading.Tasks;
using D3SK.NetCore.Api.Models;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Extensions;
using D3SK.NetCore.Domain.Features;
using Microsoft.AspNetCore.Mvc;

namespace D3SK.NetCore.Api.Controllers
{
    public class EntityPatchControllerBase<TDomain, T, TCountQuery, TQuery, TProjectionQuery, TCreate, TUpdate, TDelete, TPatch>
        : EntityPatchControllerBase<TDomain, T, int, TCountQuery, TQuery, TProjectionQuery, TCreate, TUpdate, TDelete, TPatch>
        where TDomain : IDomain
        where T : class, IEntity<int>
        where TCountQuery : IEntityCountQuery<TDomain, T>
        where TQuery : IEntityQuery<TDomain, T>
        where TProjectionQuery : IEntityProjectionQuery<TDomain, T>
        where TCreate : IEntityCreateCommand<TDomain, T>
        where TUpdate : IEntityUpdateCommand<TDomain, T>
        where TDelete : IEntityDeleteCommand<TDomain, T, int>
        where TPatch : IEntityPatchCommand<TDomain, T>
    {
        protected EntityPatchControllerBase(IDomainInstance<TDomain> instance, IExceptionManager exceptionManager)
            : base(instance, exceptionManager)
        {
        }
    }

    public class EntityPatchControllerBase<TDomain, T, TKey, TCountQuery, TQuery, TProjectionQuery, TCreate, TUpdate, TDelete, TPatch>
        : EntityControllerBase<TDomain, T, TKey, TCountQuery, TQuery, TProjectionQuery, TCreate, TUpdate, TDelete>
        where TDomain : IDomain
        where T : class, IEntity<TKey>
        where TCountQuery : IEntityCountQuery<TDomain, T>
        where TQuery : IEntityQuery<TDomain, T>
        where TProjectionQuery : IEntityProjectionQuery<TDomain, T>
        where TCreate : IEntityCreateCommand<TDomain, T>
        where TUpdate : IEntityUpdateCommand<TDomain, T>
        where TDelete : IEntityDeleteCommand<TDomain, T, TKey>
        where TPatch : IEntityPatchCommand<TDomain, T>
    {
        protected EntityPatchControllerBase(IDomainInstance<TDomain> instance, IExceptionManager exceptionManager)
            : base(instance, exceptionManager)
        {
        }

        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public virtual async Task<IActionResult> PatchEntity([FromServices] TPatch command,
            [FromRoute] TKey id, [FromBody] EntityPatchCommandRequest<T> request)
        {
            if (!request.CurrentItem.Id.Equals(id)) return BadRequest();
            if (!request.PropertiesToUpdate?.Any() ?? true) return BadRequest();

            request.SetCommand(command);
            await DomainInstance.RunCommandAsync(command);
            return NoContent();
        }
    }
}
