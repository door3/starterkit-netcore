using System;
using System.Collections.Generic;
using System.Text;

namespace D3SK.NetCore.Common.Systems
{
    public abstract class ExecuteProcessResult
    {
        public bool IsSuccessful { get; }

        public int RecordsCreated { get; }

        public int RecordsUpdated { get; }

        public int RecordsDeleted { get; }

        protected ExecuteProcessResult(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
    }

    public class SuccessfulProcessResult : ExecuteProcessResult
    {
        public SuccessfulProcessResult() : base(true)
        {
        }
    }

    public class UnsuccessfulProcessResult : ExecuteProcessResult
    {
        public UnsuccessfulProcessResult() : base(false)
        {
        }
    }

    public class IsExecutingProcessResult : ExecuteProcessResult
    {
        public IsExecutingProcessResult() : base(false)
        {
        }
    }
}
