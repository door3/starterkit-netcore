﻿using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Domain.Stores;

namespace D3SK.NetCore.Domain.Features
{
    public interface IEntityProjectionQuery : IProjectionStoreQueryFeature
    {
    }

    public interface IEntityProjectionQuery<TDomain> : IAsyncQueryFeature<TDomain, IList<object>>,
        IProjectionStoreQueryFeature
        where TDomain : IDomain
    {
    }
}