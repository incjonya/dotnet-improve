using System;
using System.Linq;
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
    }
}
