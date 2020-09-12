using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3SK.NetCore.Common.Systems
{
    public interface IProcessBase
    {
        bool IsExecuting { get; }

        string GetProcessName();
    }

    public interface IProcess<in TOptions> : IProcessBase where TOptions : ExecuteProcessOptions
    {
        
        Task<ExecuteProcessResult> ExecuteAsync(TOptions options);
    }
}
