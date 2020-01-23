using System.Collections.Generic;
using System.Linq;
using Bogus;
using Common;
using Xunit;

namespace Queue.Common.Tests
{
    public class XQueueTest
    {
        [Fact]
        public void TestJoinOneElement()
        {
            var (source, join) = Prepare();

            //we generated source with ids order as 0,1,2. For the check we take element with ID = 2, its index is 2
            var actual = XLeftJoin.Get(source, join).ToList()[2];

            //we generated join with ids order as 2,4. For the check we take element with ID = 2, its index is 0
            var expected = join.ToList()[0];

            Assert.Equal(expected.Details, actual.Details);
        }

        [Fact]
        public void TestJoinMultiple()
        {
            var (source, join) = Prepare();

            var actual = XLeftJoin.Get(source, join);

            var first = actual.ToList();
            Assert.Equal(3, first.Count);

            var second = actual.ToList();
            Assert.Equal(3, second.Count);           
        }

        [Fact]
        public void TestJoinNullReference()
        {
            var (source, join) = Prepare();

            var actual = XLeftJoin.Get(source, join);

            var first = actual.ToList();

            var nullEl2 = first.GetEnumerator().Current;
            Assert.Null(nullEl2);

            var nullEl = actual.GetEnumerator().Current;
            Assert.Null(nullEl);
        }

        [Fact]
        public void TestJoinOneElementYield()
        {
            var (source, join) = Prepare();

            //we generated source with ids order as 0,1,2. For the check we take element with ID = 2, its index is 2
            var actual = XLeftJoin.GetYield(source, join).ToList()[2];

            //we generated join with ids order as 2,4. For the check we take element with ID = 2, its index is 0
            var expected = join.ToList()[0];

            Assert.Equal(expected.Details, actual.Details);
        }

        private (IEnumerable<Item1> source, IEnumerable<Item2> join) Prepare()
        {
            var ids = 0;
            var sourceFaker = new Faker<Item1>()
                .RuleFor(o => o.ID, f => ids++)
                .RuleFor(o => o.Value, f => f.Random.Number(-1000, 1000));

            var source = sourceFaker.Generate(3);

            ids = 0;

            var joinFaker = new Faker<Item2>()
                .RuleFor(o => o.ID, f => ids += 2)
                .RuleFor(o => o.Details, f => f.Name.FirstName());

            var join = joinFaker.Generate(2);

            return (source, join);
        }
    }
}
