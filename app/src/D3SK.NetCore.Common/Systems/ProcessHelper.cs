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
            => RunProcessesAsync(options, true, new List<Type>(), processes);

        public static Task RunProcessesAsync<TOptions>(TOptions options, IList<Type> alreadyRunProcessTypes, params IProcess<TOptions>[] processes)
            where TOptions : ExecuteProcessOptions
            => RunProcessesAsync(options, true, alreadyRunProcessTypes, processes);

        public static async Task RunProcessesAsync<TOptions>(TOptions options, bool isSequential, 
            IList<Type> alreadyRunProcessTypes, params IProcess<TOptions>[] processes)
            where TOptions : ExecuteProcessOptions
        {
            if (isSequential)
            {
                var currentProcessTypes = new List<Type>(alreadyRunProcessTypes);
                processes.ToList().ForEach(proc =>
                {
                    if (!CheckProcessDependencies(proc, currentProcessTypes))
                    {
                        throw new ProcessDependencyException(
                            $"Process dependency for {proc.GetType().Name} has not been run.");
                    }

                    currentProcessTypes.Add(proc.GetType());
                });
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

        public static bool CheckProcessDependencies(IProcessBase process, IList<Type> currentProcessTypes)
        {
            var dependencies = process.GetProcessDependencies();
            return dependencies.All(x => currentProcessTypes.Any(y => y.InheritsFrom(x)));
        }
    }
}
