using System;
using System.Linq;
using Bogus;
using Common;
using Common.Tests;

namespace ConsoleDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary_Foreach();
        }

        public static void Dictionary_Foreach()
        {
            var d = new XDictionary<TestKey, int>();
            for (int i = -100000; i < 100000; i++)
            {
                d.Add(new TestKey(i), i);
            }
            var items = d.OrderBy(x => x.Key);
            var j = -100000;
            foreach (var item in items)
            {
                Console.WriteLine($"k={item.Key.InternalKey}, v={item.Value}, j={j}");
                j++;
            }
        }
    }
}
