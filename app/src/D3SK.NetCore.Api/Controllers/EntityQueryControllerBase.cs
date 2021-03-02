using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
    public class EntityQueryControllerBase<TDomain, T, TCountQuery, TQuery>
        : EntityQueryControllerBase<TDomain, T, int, TCountQuery, TQuery>
        where TDomain : IDomain
        where T : class, IEntity<int>
        where TCountQuery : IEntityCountQuery<TDomain, T>
        where TQuery : IEntityQuery<TDomain, T>
    {
        protected EntityQueryControllerBase(IDomainInstance<TDomain> instance, IExceptionManager exceptionManager)
            : base(instance, exceptionManager)
        {
        }
    }

    public class EntityQueryControllerBase<TDomain, T, TKey, TCountQuery, TQuery>
        : DomainControllerBase<TDomain>
        where TDomain : IDomain
        where T : class, IEntity<TKey>
        where TCountQuery : IEntityCountQuery<TDomain, T>
        where TQuery : IEntityQuery<TDomain, T>
    {
        protected EntityQueryControllerBase(IDomainInstance<TDomain> instance, IExceptionManager exceptionManager)
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
            query.NonOwnerPermissionId = GetNonOwnerPermissionId();
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
            query.NonOwnerPermissionId = GetNonOwnerPermissionId();
            request?.SetQueryAndCount(query, countQuery);
            return Ok(await SearchCore(query, countQuery));
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetEntityById([FromServices] TQuery query, [FromRoute] TKey id)
        {
            query.NonOwnerPermissionId = GetNonOwnerPermissionId();
            query.Filters.Add(new QueryFilter(id));
            return Ok((await DomainInstance.RunQueryAsync(query)).SingleOrDefault());
        }

        protected virtual async Task<SearchQueryResponse<TResult>> SearchCore<TResult>(
            IAsyncQueryFeature<TDomain, IList<TResult>> query,
            IEntityCountQuery<TDomain, T> countQuery)
        {
            var result = await DomainInstance.RunQueryAsync(query);
            var count = await DomainInstance.RunQueryAsync(countQuery);
            return new SearchQueryResponse<TResult>(result, count);
        }

        protected virtual int GetNonOwnerPermissionId() => 0;
    }
}
