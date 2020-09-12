using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using Microsoft.Extensions.Logging;

namespace D3SK.NetCore.Common.Systems
{
    public abstract class ProcessBase<TOptions> : IProcess<TOptions> 
        where TOptions : ExecuteProcessOptions
    {
        protected readonly ILogger Logger;

        protected string CurrentStepName { get; set; }

        public bool IsExecuting { get; protected set; }

        public string GetProcessName() => GetType().Name;

        protected string GetProcessAndStepName() => $"{GetProcessName().AppendWord(CurrentStepName, ".")}";

        protected ProcessBase(ILogger logger)
        {
            Logger = logger.NotNull(nameof(logger));
        }

        public async Task<ExecuteProcessResult> ExecuteAsync(TOptions options)
        {
            if (IsExecuting) return new IsExecutingProcessResult();

            try
            {
                IsExecuting = true;
                var result = await OnExecuteAsync(options);
                return result;
            }
            finally
            {
                IsExecuting = false;
            }
        }

        protected abstract Task<ExecuteProcessResult> OnExecuteAsync(TOptions options);

        protected async Task<ExecuteProcessResult> ExecuteStepAsync(string stepName,
            Func<Task<ExecuteProcessResult>> stepFunc)
        {
            CurrentStepName = stepName;
            Logger.LogInformation($"Executing step {GetProcessName()}.{stepName}...");

            var result = await stepFunc();

            Logger.LogInformation(
                $"Executed step {GetProcessName()}.{stepName}...");

            return result;
        }
    }
}
