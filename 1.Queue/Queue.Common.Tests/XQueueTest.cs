using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Xunit;

namespace Queue.Common.Tests
{
    public class XQueueTest
    {
        [Fact]
        public void TestCount()
        {
            var q = new XQueue<Object>();

            object i = 10;

            q.Enqueue(i);

            Assert.Equal(1, q.Count);
        }


        [Fact]
        public void TestQueue()
        {
            var q = new XQueue<Object>();

            object i = 10;

            q.Enqueue(i);
            var j = q.Dequeue();

            Assert.Equal(i, j);
            Assert.Equal(0, q.Count);
        }

        [Fact]
        public void TestQueueMany()
        {
            var q = new XQueue<Object>();

            object[] data = Enumerable.Range(1, 20).Select(i => (object)i).ToArray();

            foreach(var i in data)
                q.Enqueue(i);


            for(int i =0; i< data.Length; i++)
            {
                var j = q.Dequeue();
                Assert.Equal(data[i], j);
            }
            
            Assert.Equal(0, q.Count);
        }

        [Fact]
        public void TestNoElements()
        {
            Assert.Throws<ArgumentException>(() => (new XQueue<Object>()).Dequeue());
        }

        [Fact]
        public async Task TestHighLoad()
        {
            var q = new XQueue<int>();

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var tEnq = Task.Run(() =>
            {
                for (int i = 0; i < 1_000_000; i++)
                    q.Enqueue(i);
            });

            var tEnq2 = Task.Run(() =>
            {
                for (int i = 0; i < 1_000_000; i++)
                    q.Enqueue(i);
            });


            //await Task.Delay(30);

            //var tDeq = Task.Run(() =>
            //{
            //    for (int i = 0; i < 1_000_000; i++)
            //        q.Dequeue();
            //});

            //var tDeq2 = Task.Run(() =>
            //{
            //    for (int i = 0; i < 1_000_000; i++)
            //        q.Dequeue();
            //});

            await Task.WhenAll(tEnq, tEnq2);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Assert.Equal(2_000_000, q.Count);
            Assert.True(elapsedMs <= 500);
        }
    }
}
