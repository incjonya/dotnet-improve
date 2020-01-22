using System;
using System.Linq;
using System.Threading.Tasks;
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

            //we generated source with ids order as 0,1,2. For the check we take element with ID = 2, its index is 2
            var actual = XLeftJoin.Get(source, join).ToList()[2];

            //we generated join with ids order as 2,4. For the check we take element with ID = 2, its index is 0
            var expected = join.ToList()[0];

            Assert.Equal(expected.Details, actual.Details);
        }
    }
}
