using D3SK.NetCore.Common.Entities;
using D3SK.NetCore.Domain;
using System.Collections.Generic;
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

    public class EntityPatchCommandRequest<T> : CommandRequestBase<IEntityPatchCommand<T>> where T : class, IEntityBase
    {
        public T OriginalItem { get; set; }

        public T CurrentItem { get; set; }

        public IList<string> PropertiesToUpdate { get; set; } = new List<string>();

        public override void SetCommand(IEntityPatchCommand<T> command)
        {
            command.NotNull(nameof(command));
            command.OriginalItem = OriginalItem;
            command.CurrentItem = CurrentItem;
            command.PropertiesToUpdate = PropertiesToUpdate;
        }
    }

    public class EntityMultiplePatchCommandRequest<T>
        : CommandRequestBase<IEntityMultiplePatchCommand<T>> where T : class, IEntityBase
    {
        public Item<T>[] Items { get; set; }

        public IList<string> PropertiesToUpdate { get; set; } = new List<string>();

        public override void SetCommand(IEntityMultiplePatchCommand<T> command)
        {
            command.NotNull(nameof(command));
            command.Items = Items;
            command.PropertiesToUpdate = PropertiesToUpdate;
        }
    }

    public class EntityMultipleCreateCommandRequest<T>
        : CommandRequestBase<IEntityMultipleCreateCommand<T>> where T : class, IEntityBase
    {
        public T[] Items { get; set; }

        public override void SetCommand(IEntityMultipleCreateCommand<T> command)
        {
            command.NotNull(nameof(command));
            command.Items = Items;
        }
    }
}
