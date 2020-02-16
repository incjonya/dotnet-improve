using System;
using Common;

namespace ConsoleDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            var q = new XQueue<int>();
            using (var conW = new ConsoleQueueWatcher<int>(q))
            {
                using (var logW = new LogQueueWatcher<int>(q))
                {
                    Console.WriteLine($"Start queue process, watchers: {q.WatchersInfo}");

                    for (int i = 0; i < 10; i++)
                    {
                        q.Enqueue(i);
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        var _ = q.Dequeue();
                    }
                }
            }

            Console.WriteLine($"Queue process disposed, watchers: {q.WatchersInfo}");

            q.Enqueue(10);
            var res = q.Dequeue();

            Console.WriteLine($"vaue {res} was removed without watching");

            Console.ReadKey();
        }
    }
}
