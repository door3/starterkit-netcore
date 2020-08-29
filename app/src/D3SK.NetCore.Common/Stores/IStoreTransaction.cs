using System;

namespace D3SK.NetCore.Common.Stores
{
    public interface IStoreTransaction : IDisposable
    {
        event EventHandler Committed;

        event EventHandler RolledBack;

        bool IsCommitted { get; }

        bool IsRolledBack { get; }

        bool IsComplete { get; }

        void Commit();

        void Rollback();

        object GetTransaction();
    }
}
