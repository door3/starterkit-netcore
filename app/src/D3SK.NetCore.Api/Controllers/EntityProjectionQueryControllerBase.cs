using System;
using System.Collections.Generic;
using System.Linq;
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
    public class EntityProjectionQueryControllerBase<TDomain, T, TCountQuery, TQuery, TProjectionQuery>
        : EntityProjectionQueryControllerBase<TDomain, T, int, TCountQuery, TQuery, TProjectionQuery>
        where TDomain : IDomain
        where T : class, IEntity<int>
        where TCountQuery : IEntityCountQuery<TDomain, T>
        where TQuery : IEntityQuery<TDomain, T>
        where TProjectionQuery : IEntityProjectionQuery<TDomain, T>
    {
        protected EntityProjectionQueryControllerBase(IDomainInstance<TDomain> instance, IExceptionManager exceptionManager)
            : base(instance, exceptionManager)
        {
        }
    }

    public class EntityProjectionQueryControllerBase<TDomain, T, TKey, TCountQuery, TQuery, TProjectionQuery>
        : EntityQueryControllerBase<TDomain, T, TKey, TCountQuery, TQuery>
        where TDomain : IDomain
        where T : class, IEntity<TKey>
        where TCountQuery : IEntityCountQuery<TDomain, T>
        where TQuery : IEntityQuery<TDomain, T>
        where TProjectionQuery : IEntityProjectionQuery<TDomain, T>
    {
        protected EntityProjectionQueryControllerBase(IDomainInstance<TDomain> instance, IExceptionManager exceptionManager)
            : base(instance, exceptionManager)
        {
        }

        [HttpPost("select")]
        public virtual async Task<IActionResult> SelectEntities(
            [FromServices] TProjectionQuery query,
            [FromServices] TCountQuery countQuery,
            [FromBody] FromBodyProjectionSearchQueryRequest request)
        {
            query.NonOwnerPermissionId = GetNonOwnerPermissionId();
            countQuery.NonOwnerPermissionId = GetNonOwnerPermissionId();
            request?.SetQueryAndCount(query, countQuery);
            return Ok(await SearchCore(query, countQuery));
        }
    }
}
