using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleDebug
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Example1();

            //Example2();

            //Example3();

            //Example4();

            Example5();

            Console.ReadKey();
        }

        #region Example 1

        /// <summary>
        /// В чем разница 2ух вариантов континуэйшена
        /// </summary>
        private static void Example1()
        {
            var v = new TaskJob();

            v.ContinueOne();

            v.ContinueTwo();
        }

        #endregion

        #region Example 2

        /// <summary>
        /// Что выведется и в каких тредах
        /// </summary>
        /// <returns></returns>
        private static async Task Example2()
        {
            var t = new TaskAwaiter();

            _ = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} set completed");
                t.SetCompleted();
            });

            while (true)
            {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} before await");
                var resutl = await t;
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} result {resutl}");
            }
        }

        #endregion

        #region Example 3

        private static readonly TaskCompletionSource<bool> completion = new TaskCompletionSource<bool>();

        /// <summary>
        /// Что будет выведено на консоль
        /// </summary>
        /// <returns></returns>
        private static async Task Example3()
        {
            "1".WriteLine();
            _ = Complete();
            await completion.Task.ConfigureAwait(false);
            "And we're done".WriteLine();
        }

        private static async Task Complete()
        {
            await Task.Delay(1_000);
            "2".WriteLine();
            completion.SetResult(true);
            "3".WriteLine();
            "4".WriteLine();
        }

        #endregion

        #region Example 4

        /// <summary>
        /// Чтоб будет и почему
        /// </summary>
        private static async void Example4()
        {
            var v = new AsyncJob();

            var res = await v.Index();

            res.WriteLine();
        }

        #endregion

        #region Example 5

        private static void Example5()
        {
            "Main Start".WriteLine();
            SourceJob.MainDo();
        }

        #endregion
    }
}
