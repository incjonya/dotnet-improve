using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Console.WriteLine($"Dictionary:");
            Dictionary_Empty_AddKey(new Dictionary<TestKey, int>(), 100000);

            Console.WriteLine($"XDictionary:");
            XDictionary_Empty_AddKey(new XDictionary<TestKey, int>(), 100000);
        }

        public static void XDictionary_Empty_AddKey(IMyDictionary<TestKey, int> d, int size)
        {
            var sw = new Stopwatch();
            sw.Start();
            
            Console.WriteLine($"test started");
            
            for (int i = 0; i < size; i++)
            {
                d.Add(new TestKey(i), i);

                if (i % 1000 == 0)
                    Console.WriteLine($"{i/1000}% done in {sw.ElapsedMilliseconds / 1000} sec");
            }
            Console.WriteLine($"count = {d.Count} in {sw.ElapsedMilliseconds / 1000} sec");
        }

        public static void Dictionary_Empty_AddKey(IDictionary<TestKey, int> d, int size)
        {
            var sw = new Stopwatch();
            sw.Start();

            Console.WriteLine($"test started");
            
            for (int i = 0; i < size; i++)
            {
                d.Add(new TestKey(i), i);

                if (i % 1000 == 0)
                    Console.WriteLine($"{i / 1000}% done in {sw.ElapsedMilliseconds / 1000} sec");
            }
            Console.WriteLine($"count = {d.Count} in {sw.ElapsedMilliseconds / 1000} sec");
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
