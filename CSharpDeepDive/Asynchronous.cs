namespace CSharpDeepDive
{
    /// <summary>
    /// Demonstrates various asynchronous programming patterns and practices in C#.
    /// Includes both good and bad examples to illustrate proper usage of Task-based methods.
    /// </summary>
    public class Asynchronous
    {
        /// <summary>
        /// Simulates a long-running asynchronous operation using Task.Delay.
        /// </summary>
        public async Task<(string Result, int ThreadId, string? ThreadName)> GetDataAsyncMethod()
        {
            await Task.Delay(3000);
            Console.WriteLine("Completed Method 1");
            return ("Result1", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name);
        }

        /// <summary>
        /// Simulates a faster asynchronous operation.
        /// </summary>
        public async Task<(string Result, int ThreadId, string? ThreadName)> GetDataAsyncMethod2()
        {
            Console.WriteLine("Started Method 2");
            await Task.Delay(1000);
            Console.WriteLine("Completed Method 2");
            return ("Result2", Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name);
        }

        /// <summary>
        /// Proper use of Task.Run to offload CPU-bound work.
        /// </summary>
        public Task<string> GetDataAsync()
        {
            return Task.Run(() =>
            {
                return "Data retrieved";
            });
        }

        /// <summary>
        /// Immediate result wrapped in a Task using Task.FromResult.
        /// </summary>
        public Task<string> GetDataAsync2(string param)
        {
            return Task.FromResult(param);
        }

        /// <summary>
        /// BAD EXAMPLE: Redundant Task.Run call and unused result.
        /// </summary>
        public Task<List<string>> GetDataAsync3(string param)
        {
            var res = param.Where(x => x == 'a').Select(x => x.ToString()).ToList();
            Task.Run(() => res);
            return Task.FromResult(res);
        }

        /// <summary>
        /// OK EXAMPLE: Wraps precomputed data in Task.Run unnecessarily.
        /// </summary>
        public Task<List<string>> GetDataAsync4(string param)
        {
            var res = param.Where(x => x == 'a').Select(x => x.ToString()).ToList();
            return Task.Run(() => res);
        }

        /// <summary>
        /// GOOD EXAMPLE: Proper Task.Run usage for computation.
        /// </summary>
        public Task<List<string>> GetDataAsync5(string param)
        {
            return Task.Run(() =>
            {
                var res = param.Where(x => x == 'a').Select(x => x.ToString()).ToList();
                return res;
            });
        }

        /// <summary>
        /// Demonstrates Task.WhenAny: returns the first completed task's result.
        /// </summary>
        public async Task<(string Result, int ThreadId, string? ThreadName)> GetDataAsyncWhenAny()
        {
            var completedTask = await Task.WhenAny(GetDataAsyncMethod2(), GetDataAsyncMethod());
            return completedTask.Result;
        }

        /// <summary>
        /// Demonstrates Task.WhenAll: awaits all tasks and returns the first result.
        /// </summary>
        public async Task<(string Result, int ThreadId, string? ThreadName)> GetDataAsyncWhenAll()
        {
            var completedTasks = await Task.WhenAll(GetDataAsyncMethod(), GetDataAsyncMethod2());
            return completedTasks.FirstOrDefault();
        }

        /// <summary>
        /// BAD EXAMPLE: Blocking call using .Result (synchronous wait on async).
        /// </summary>
        public string GetDataWithResultBlocking()
        {
            return GetDataAsync2("blocking").Result;
        }

        /// <summary>
        /// BAD EXAMPLE: Blocking call using .Wait().
        /// </summary>
        public void GetDataWithWait()
        {
            GetDataAsync2("waiting").Wait();
        }

        /// <summary>
        /// BAD EXAMPLE: Calling async method synchronously using .GetAwaiter().GetResult().
        /// </summary>
        public string GetDataWithGetAwaiter()
        {
            return GetDataAsync2("awaiter").GetAwaiter().GetResult();
        }

        /// <summary>
        /// GOOD EXAMPLE: Fully asynchronous method that avoids blocking.
        /// </summary>
        public async Task<string> GetDataWithoutBlockingAsync()
        {
            var result = await GetDataAsync2("non-blocking");
            return result;
        }

        /// <summary>
        /// BAD EXAMPLE: Fire-and-forget with no error handling.
        /// </summary>
        public void FireAndForgetBad()
        {
            _ = GetDataAsync();
        }

        /// <summary>
        /// GOOD EXAMPLE: Fire-and-forget with exception safety.
        /// </summary>
        public void FireAndForgetSafe()
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    var result = await GetDataAsync();
                    Console.WriteLine($"Fire-and-forget result: {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception caught in fire-and-forget: {ex.Message}");
                }
            });
        }

        /// <summary>
        /// BAD EXAMPLE: Mixing async with blocking loops - CPU-bound in async method.
        /// </summary>
        public async Task<string> HeavyLoopAsync()
        {
            for (int i = 0; i < 10000000; i++)
            {
                // Blocking work
            }
            return await Task.FromResult("done");
        }

        /// <summary>
        /// GOOD EXAMPLE: CPU-bound work offloaded to background thread.
        /// </summary>
        public Task<string> HeavyLoopWithTaskRun()
        {
            return Task.Run(() =>
            {
                for (int i = 0; i < 10000000; i++)
                {
                    // Background work
                }
                return "done";
            });
        }

        /// <summary>
        /// GOOD PRACTICE: Configures await to not capture synchronization context.
        /// </summary>
        public async Task<string> GetDataWithConfigureAwait()
        {
            var result = await GetDataAsync2("context-free").ConfigureAwait(false);
            return result;
        }
    }
}
