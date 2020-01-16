using System;
using Common;

namespace ConsoleDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            var q = new XQueue<int>();
            q.Enqueue(10);
            
            Console.WriteLine($"Value added to queue. Queue size = {q.Count}");
            var i = q.Dequeue();
            Console.WriteLine($"Value = {i} removed from queue, Queue size = {q.Count}");
            Console.ReadKey();
        }
    }
}
