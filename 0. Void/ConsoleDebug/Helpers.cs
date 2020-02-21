using System;
using System.Threading;

namespace ConsoleDebug
{
    public static class Helpers
    {
        public static void WriteLine(this object s)
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: {s}");
        }
    }
}
