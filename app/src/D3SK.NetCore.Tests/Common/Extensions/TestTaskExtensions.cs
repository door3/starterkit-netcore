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
        public async Task WithActionTestAsync()
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

        [Fact]
        public async Task AsTaskTestAsync()
        {
            const string testString = "This is a test string";

            // test object to object
            await AssertTestAsync(((object) testString).AsTask());

            // test object to type
            await AssertTestAsync(((object) testString).AsTask<string>());

            // test type
            await AssertTestAsync(testString.AsTask());
            
            async Task AssertTestAsync<TTaskType>(Task<TTaskType> testTask)
            {
                await Assert.IsType<Task<TTaskType>>(testTask);
                var testTaskResult = await testTask;
                Assert.IsType<string>(testTaskResult);
                Assert.Equal(testString, testTaskResult.ToString());
            }
        }

        [Fact]
        public async Task TaskActionRunTestAsync()
        {
            var testActionInt = 0;

            var testTask = TaskAction.Run(() => testActionInt += 92);
            // make sure a completed task is return, and the int was updated
            Assert.Equal(Task.CompletedTask, testTask);
            Assert.Equal(92, testActionInt);
            // make sure awaiting the task doesn't actually do anything (like rerun the action)
            await testTask;
            Assert.Equal(Task.CompletedTask, testTask);
            Assert.Equal(92, testActionInt);
        }
    }
}
