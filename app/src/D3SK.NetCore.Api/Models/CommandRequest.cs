using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Domain.Features;

namespace D3SK.NetCore.Api.Models
{
    public abstract class CommandRequestBase<TCommand> where TCommand : ICommandFeature
    {
        public abstract void SetCommand(TCommand command);
    }

    public class EntityCreateCommandRequest<T> : CommandRequestBase<IEntityCreateCommand<T>> where T : class, IEntityBase
    {
        public T CurrentItem { get; set; }

        public override void SetCommand(IEntityCreateCommand<T> command)
        {
            command.NotNull(nameof(command));
            command.CurrentItem = CurrentItem;
        }
    }

    public class EntityCreateCommandRequest<T, TKey> : EntityCreateCommandRequest<T>
        where T : class, IEntity<TKey>
    {
    }

    public class EntityUpdateCommandRequest<T> : CommandRequestBase<IEntityUpdateCommand<T>> where T : class, IEntityBase
    {
        public T OriginalItem { get; set; }

        public T CurrentItem { get; set; }

        public override void SetCommand(IEntityUpdateCommand<T> command)
        {
            command.NotNull(nameof(command));
            command.OriginalItem = OriginalItem;
            command.CurrentItem = CurrentItem;
        }
    }

    public class EntityUpdateCommandRequest<T, TKey> : EntityUpdateCommandRequest<T>
        where T : class, IEntity<TKey>
    {
    }
}
