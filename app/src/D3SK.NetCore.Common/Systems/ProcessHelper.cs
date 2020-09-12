using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;

namespace D3SK.NetCore.Common.Systems
{
    public static class ProcessHelper
    {
        public static Task RunProcessesAsync<TOptions>(TOptions options, params IProcess<TOptions>[] processes)
            where TOptions : ExecuteProcessOptions
            => RunProcessesAsync(options, true, processes);

        public static async Task RunProcessesAsync<TOptions>(TOptions options, bool isSequential,
            params IProcess<TOptions>[] processes)
            where TOptions : ExecuteProcessOptions
        {
            if (isSequential)
            {
                await processes.ForEachAsync(proc => RunProcessAsync(proc, options));
            }
            else
            {
                await Task.WhenAll(processes.Select(proc => RunProcessAsync(proc, options)));
            }
        }

        public static async Task<ExecuteProcessResult> RunProcessAsync<TOptions>(IProcess<TOptions> process,
            TOptions options)
            where TOptions : ExecuteProcessOptions
        {
            var result = await process.ExecuteAsync(options);

            return result;
        }
    }
}
