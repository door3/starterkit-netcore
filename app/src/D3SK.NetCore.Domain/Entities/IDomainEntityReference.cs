﻿using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Domain.Entities
{
    public interface IDomainEntityReference : IDomainEntityReference<int>
    {
    }

    public interface IDomainEntityReference<out TKey> : IEntityReference<TKey>
    {
        [Newtonsoft.Json.JsonIgnore]
        string ExtendedDataJson { get; }
    }
}
