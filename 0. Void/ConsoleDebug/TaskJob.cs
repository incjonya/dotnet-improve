using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDebug
{
    public class TaskJob
    {
        public void ContinueOne()
        {
            var t = Task.Run(() => Console.WriteLine("It's a task - 1"));
            t = t.ContinueWith(_ => Console.WriteLine("continue - 1"));
        }

        public async Task ContinueTwo()
        {
            var t = Task.Run(() => Console.WriteLine("It's a task - 2"));
            await t;
            Console.WriteLine("continue - 2");
        }
    }
}
