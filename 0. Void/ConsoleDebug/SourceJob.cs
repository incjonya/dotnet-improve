using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleDebug
{
    public static class SourceJob
    {
        private static TaskCompletionSource<object> _preInit1 = new TaskCompletionSource<object>();
        private static TaskCompletionSource<object> _preInit2 = new TaskCompletionSource<object>(TaskContinuationOptions.RunContinuationsAsynchronously);

        public static async Task RunOperation1()
        {
            "PreInit 1".WriteLine();
            await Task.Delay(2000);
            _preInit1.SetResult(null);

            await _preInit2.Task;
            "Run 1".WriteLine();

            for (; ; ) {
               // WriteLine("Do work 1");
            };
        }

        public static async Task RunOperation2()
        {
            "PreInit 2".WriteLine();
            await Task.Delay(5000);
            _preInit2.SetResult(null);

            await _preInit1.Task;
            "Run 2".WriteLine();

            for (; ; );
        }

        public static async Task MainDo()
        {
            var t1 = RunOperation1();
            "some text".WriteLine();
            var t2 = RunOperation2();

            await Task.WhenAll(t1, t2);
        }
    }
}
