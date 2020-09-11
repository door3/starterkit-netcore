using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using D3SK.NetCore.Api.Models;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Queries;
using D3SK.NetCore.Common.Utilities;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Extensions;
using D3SK.NetCore.Domain.Features;
using Microsoft.AspNetCore.Mvc;

namespace D3SK.NetCore.Api.Controllers
{
    public class EntityControllerBase<TDomain, T, TCountQuery, TQuery, TProjectionQuery, TCreate, TUpdate, TDelete>
        : EntityControllerBase<TDomain, T, int, TCountQuery, TQuery, TProjectionQuery, TCreate, TUpdate, TDelete>
        where TDomain : IDomain
        where T : class, IEntity<int>
        where TCountQuery : IEntityCountQuery<TDomain, T>
        where TQuery : IEntityQuery<TDomain, T>
        where TProjectionQuery : IEntityProjectionQuery<TDomain, T>
        where TCreate : IEntityCreateCommand<TDomain, T>
        where TUpdate : IEntityUpdateCommand<TDomain, T>
        where TDelete : IEntityDeleteCommand<TDomain, T, int>
    {
        protected EntityControllerBase(IDomainInstance<TDomain> instance, IExceptionManager exceptionManager)
            : base(instance, exceptionManager)
        {
        }
    }

    public class EntityControllerBase<TDomain, T, TKey, TCountQuery, TQuery, TProjectionQuery, TCreate, TUpdate, TDelete>
        : DomainControllerBase<TDomain>
        where TDomain : IDomain
        where T : class, IEntity<TKey>
        where TCountQuery: IEntityCountQuery<TDomain, T>
        where TQuery : IEntityQuery<TDomain, T>
        where TProjectionQuery : IEntityProjectionQuery<TDomain, T>
        where TCreate : IEntityCreateCommand<TDomain, T>
        where TUpdate : IEntityUpdateCommand<TDomain, T>
        where TDelete : IEntityDeleteCommand<TDomain, T, TKey>
    {
        protected EntityControllerBase(IDomainInstance<TDomain> instance, IExceptionManager exceptionManager)
            : base(instance, exceptionManager)
        {
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetEntities(
            [FromServices] TQuery query,
            [FromServices] TCountQuery countQuery,
            [FromQuery] FromQuerySearchQueryRequest request,
            [FromQuery] int? id)
        {
            request?.SetQueryAndCount(query, countQuery);
            if (id.HasValue)
            {
                query.Filters.Add(new QueryFilter(id));
                return Ok((await DomainInstance.RunQueryAsync(query)).SingleOrDefault());
            }

            return Ok(await SearchCore(query, countQuery));
        }

        [HttpPost("search")]
        public virtual async Task<IActionResult> SearchEntities(
            [FromServices] TQuery query,
            [FromServices] TCountQuery countQuery,
            [FromBody] FromBodySearchQueryRequest request)
        {
            request?.SetQueryAndCount(query, countQuery);
            return Ok(await SearchCore(query, countQuery));
        }

        [HttpPost("select")]
        public virtual async Task<IActionResult> SelectEntities(
            [FromServices] TProjectionQuery query,
            [FromServices] TCountQuery countQuery,
            [FromBody] FromBodyProjectionSearchQueryRequest request)
        {
            request?.SetQueryAndCount(query, countQuery);
            return Ok(await SearchCore(query, countQuery));
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetEntityById([FromServices] TQuery query, [FromRoute] int id)
        {
            query.Filters.Add(new QueryFilter(id));
            return Ok((await DomainInstance.RunQueryAsync(query)).SingleOrDefault());
        }
        
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public virtual async Task<IActionResult> CreateEntity([FromServices] TCreate command,
            [FromBody] EntityCreateCommandRequest<T> request, ApiVersion version)
        {
            request.SetCommand(command);
            await DomainInstance.RunCommandAsync(command);
            return CreatedAtRoute(new { id = command.CurrentItem.Id, version = $"{version}" },
                request.CurrentItem);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public virtual async Task<IActionResult> UpdateEntity([FromServices] TUpdate command,
            [FromRoute] TKey id, [FromBody] EntityUpdateCommandRequest<T> request)
        {
            if (!request.CurrentItem.Id.Equals(id)) return BadRequest();
            
            request.SetCommand(command);
            await DomainInstance.RunCommandAsync(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public virtual async Task<IActionResult> DeleteEntity([FromServices] TDelete command, [FromRoute] TKey id)
        {
            command.EntityId = id;
            await DomainInstance.RunCommandAsync(command);
            return NoContent();
        }

        protected virtual async Task<SearchQueryResponse<TResult>> SearchCore<TResult>(
            IAsyncQueryFeature<TDomain, IList<TResult>> query,
            IEntityCountQuery<TDomain, T> countQuery)
        {
            var result = await DomainInstance.RunQueryAsync(query);
            var count = await DomainInstance.RunQueryAsync(countQuery);
            return new SearchQueryResponse<TResult>(result, count);
        }
    }
}
