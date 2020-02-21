using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleDebug
{
    public class AsyncJob
    {
        private async Task Method1Async() => await Task.Delay(1000);

        private async Task<int> Method2Async()
        {
            await Task.Delay(1000);
            return 42;
        }

        private async Task<int> Method3Async()
        {
            var a = Method1Async().ConfigureAwait(false);
            DoSomeWork();
            await a;
            var b = Method2Async();
            var res = b.Result;
            return res;
        }

        private void DoSomeWork()
        {
            Thread.Sleep(100);
        }

        public async Task<object> Index()
        {
            var res = await Method3Async();
            return res;
        }
    }
}
