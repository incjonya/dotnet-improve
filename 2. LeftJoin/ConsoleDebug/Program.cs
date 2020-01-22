using System.Linq;
using Bogus;
using Common;

namespace ConsoleDebug
{
    class Program
    {
        static void Main(string[] args)
        {
            var ids = 0;
            var sourceFaker = new Faker<Item1>()
                .RuleFor(o => o.ID, f => ids++)
                .RuleFor(o => o.Value, f => f.Random.Number(-1000, 1000));
                
            var source = sourceFaker.Generate(1000);

            ids = 0;

            var joinFaker = new Faker<Item2>()
                .RuleFor(o => o.ID, f => ids += 3)
                .RuleFor(o => o.Details, f => f.Name.FirstName());

            var join = joinFaker.Generate(300);

            var res = XLeftJoin.Get(source, join).ToList();
        }
    }
}
