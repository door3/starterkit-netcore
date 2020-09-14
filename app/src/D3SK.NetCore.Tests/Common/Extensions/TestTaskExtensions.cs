using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using Xunit;

namespace D3SK.NetCore.Tests.Common.Extensions
{
    public class TestTaskExtensions
    {
        [Fact]
        public async Task WithActionTest()
        {
            // test with Task.CompletedTask
            var testActionInt1 = 0;
            await Task.CompletedTask.WithAction(() => testActionInt1 = 17);
            Assert.Equal(17, testActionInt1);

            // test with another task, make sure action runs first
            var testActionInt2 = 0;
            async Task TestDelayTask(CancellationToken cancellationToken)
            {
                try
                {
                    // delay forever, must cancel token to continue
                    await Task.Delay(int.MaxValue, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                }
                finally
                {
                    testActionInt2 = 39;
                }
            }

            using var testTaskCancellationTokenSource = new CancellationTokenSource();
            var testTaskCancellationToken = testTaskCancellationTokenSource.Token;
            var testTaskResult = TestDelayTask(testTaskCancellationToken).WithAction(() => testActionInt2 = 19);
            Assert.Equal(19, testActionInt2);
            // cancel token to break delay
            testTaskCancellationTokenSource.Cancel();
            // make sure rest of task completes
            await testTaskResult;
            Assert.Equal(39, testActionInt2);
        }
    }
}
