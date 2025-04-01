

namespace CSharpDeepDive
{
    public class Asynchronous
    {

        public Task<(string Result, int ThreadId, string? ThreadName)> GetDataAsyncMethod()
        {
            var result = "result";
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var threadName = Thread.CurrentThread.Name;

            return Task.FromResult((result, threadId, threadName));
        }


        public Task<(string Result, int ThreadId, string? ThreadName)> GetDataAsyncMethod2()
        {
            var result = "result2";
            var threadId = Thread.CurrentThread.ManagedThreadId;
            var threadName = Thread.CurrentThread.Name;

            return Task.FromResult((result, threadId, threadName));
        }

        public Task<string> GetDataAsync()
        {
            return Task.Run(() =>
            {
                return "Data retrieved";
            });
        }


        public Task<string> GetDataAsync2(string param)
        {
            return Task.FromResult(param);
        }


        public Task<List<string>> GetDataAsync3(string param)
        {
            var res = param.Where(x => x == 'a').
            Select(x => x.ToString())
            .ToList();

            var res3 = Task.Run(() => res);

            return Task.FromResult(res);
        }


        public Task<List<string>> GetDataAsync4(string param)
        {
            var res = param.Where(x => x == 'a').
            Select(x => x.ToString())
            .ToList();

            return Task.Run(() => res);
        }

        public Task<List<string>> GetDataAsync5(string param)
        {
            return Task.Run(() =>
            {
                var res = param
                    .Where(x => x == 'a')
                    .Select(x => x.ToString())
                    .ToList();

                return res;
            });
        }
        public async Task<(string Result, int ThreadId, string? ThreadName)> GetDataAsyncWhenAny()
        {
            var completedTask = await Task.WhenAny(GetDataAsyncMethod(), GetDataAsyncMethod2());

            var res = await completedTask;

            return res;
        }


        public async Task<(string Result, int ThreadId, string? ThreadName)> GetDataAsyncWhenAll()
        {

            var completedTask2 = await Task.WhenAll(GetDataAsyncMethod(), GetDataAsyncMethod2());


            return completedTask2.ToList()[0];
        }

    }
}