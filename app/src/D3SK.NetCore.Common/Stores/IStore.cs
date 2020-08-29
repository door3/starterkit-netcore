using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Entities;

namespace D3SK.NetCore.Common.Stores
{
    public interface IStore : IDisposable
    {
    }

    public interface IQueryStore : IStore
    {
    }

    public interface ICommandStore : IStore
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task RunCommandAsync(string command, params object[] parameters);
    }
    
    public interface ITransactionStore : ICommandStore
    {
        bool IsInTransaction { get; }

        Task<IStoreTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        IStoreTransaction UseTransaction(IStoreTransaction transaction);

        void CommitTransaction();
    }
}
