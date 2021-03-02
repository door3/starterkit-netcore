﻿using System;
using System.Collections.Generic;
using System.Linq;
using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;
using D3SK.NetCore.Domain;
using D3SK.NetCore.Domain.Features;

namespace D3SK.NetCore.Infrastructure.Features
{
    public abstract class EntityPatchCommandBase<TDomain, T, TStore, TCommandContainer>
        : EntityPatchCommandBase<TDomain, T, int, TStore, TCommandContainer>
        where TDomain : IDomain
        where T : class, IEntity<int>
        where TStore : ITransactionStore
        where TCommandContainer : ICommandContainer<T, int, TStore>
    {
        protected EntityPatchCommandBase(TCommandContainer commandContainer, IUpdateStrategy updateStrategy)
            : base(commandContainer, updateStrategy)
        {
        }
    }

    public abstract class EntityPatchCommandBase<TDomain, T, TKey, TStore, TCommandContainer>
        : EntityUpdateCommandBase<TDomain, T, TKey, TStore, TCommandContainer>, IEntityPatchCommand<TDomain, T>
        where TDomain : IDomain
        where T : class, IEntity<TKey>
        where TStore : ITransactionStore
        where TCommandContainer : ICommandContainer<T, TKey, TStore>
    {
        public IList<string> PropertiesToUpdate { get; set; } = new List<string>();

        public bool IsPatch => PropertiesToUpdate.Any();

        protected EntityPatchCommandBase(TCommandContainer commandContainer, IUpdateStrategy updateStrategy)
            : base(commandContainer, updateStrategy)
        {
        }

        protected override UpdateEntityOptions GetUpdateOptions()
        {
            return new UpdateEntityOptions() { PropertiesToUpdate = PropertiesToUpdate };
        }

        protected UpdateEntityOptions GetUpdateOptions(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            
            var properties = PropertiesToUpdate
                .Where(t => t.StartsWith(path, ignoreCase: true, culture: null))
                .Select(t => t.Substring(path.Length, t.Length - path.Length))
                .ToArray();
            return new UpdateEntityOptions() { PropertiesToUpdate = properties };
        }

        protected bool PatchIncludesProperty(string property)
        {
            return !IsPatch || PropertiesToUpdate.Contains(property, StringComparer.OrdinalIgnoreCase);
        }
    }
}
