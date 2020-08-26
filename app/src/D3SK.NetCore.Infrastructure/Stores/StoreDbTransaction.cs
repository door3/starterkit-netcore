using System;
using D3SK.NetCore.Common;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Stores;
using Microsoft.EntityFrameworkCore.Storage;

namespace D3SK.NetCore.Infrastructure.Stores
{
    public class StoreDbTransaction : DisposableBase, IStoreTransaction
    {
        private readonly IDbContextTransaction _transaction;

        public event EventHandler Committed;

        public event EventHandler RolledBack;

        public bool IsCommitted { get; private set; }

        public bool IsRolledBack { get; private set; }

        public bool IsComplete => IsCommitted || IsRolledBack;

        public StoreDbTransaction(IDbContextTransaction transaction)
        {
            _transaction = transaction.NotNull(nameof(transaction));
        }

        public void Commit()
        {
            _transaction.Commit();
            IsCommitted = true;
            OnCommitted();
        }

        public void Rollback()
        {
            _transaction.Rollback();
            IsRolledBack = true;
            OnRolledBack();
        }

        public object GetTransaction()
        {
            return _transaction.GetDbTransaction();
        }

        protected virtual void OnCommitted()
        {
            Committed?.Invoke(this, new EventArgs());
        }

        protected virtual void OnRolledBack()
        {
            RolledBack?.Invoke(this, new EventArgs());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _transaction?.Dispose();

            base.Dispose(disposing);
        }
    }
}