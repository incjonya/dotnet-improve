using System;
using System.Linq;
using System.Threading;
using Common;

namespace ConsoleDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine($"X Thredpool:");

            var x = new XThreadPool();
            x.ActionCompleted += X_ActionCompleted;

            var rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                var num = (rnd.Next(10) + 3) * 1000;

                var id = x.QueueWork(() =>
                {
                    WriteLine($"Starting simple calculator for {num} numbers...");
                    Thread.Sleep(num);

                    if (num >= 9000)
                        throw new ArgumentException("Number is over 9000!");

                    var res = Enumerable.Range(1, num).Sum();
                    WriteLine($"Sum of first {num} numbers = {res}");
                });

                WriteLine($"Action id #{id} added to pool queue");
            }

            while (x.CurrentWorkCount > 0)
            {
                WriteLine($"{x.CurrentWorkCount} jobs left");
                Thread.Sleep(10000);
            }
            
            x.WaitForShutdown();

            WriteLine("all work items completed, press any key...");
            Console.ReadKey();
        }

        private static void WriteLine(string v)
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: {v}");
        }

        private static void X_ActionCompleted(object sender, WorkResult res)
        {
            if (res.Success)
            {
                WriteLine($"Action id #{res.WorkId} completed successfully");
            }
            else
            {
                WriteLine($"Action id #{res.WorkId} completed with error: {res.Error}");
            }
        }
    }
}
