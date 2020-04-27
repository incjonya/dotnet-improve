using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Common.Tests
{
    public class ExTests
    {
        public static IMyDictionary<TestKey, TValue> Init<TValue>() => new XDictionary<TestKey, TValue>();

        [Theory]
        [InlineData(HashType.Int)]
        [InlineData(HashType.String)]
        [InlineData(HashType.StrLen)]
        public void Dictionary_Empty_AddKey(HashType hashType)
        {
            var d = Init<int>();
            for (int i = 0; i < 1000; i++)
            {
                d.Add(new TestKey(i, hashType), i);
            }
            d.Count.Should().Be(1000);
        }

        [Theory]
        [InlineData(HashType.Int)]
        [InlineData(HashType.String)]
        [InlineData(HashType.StrLen)]
        public void Dictionary_AddDuplicateKey(HashType hashType)
        {
            var d = Init<int>();
            Action action = () =>
            {
                d.Add(new TestKey(42, hashType), 1);
                d.Add(new TestKey(42, hashType), 2);
            };
            action.Should().Throw<SameKeyException>();
        }

        [Theory]
        [InlineData(HashType.Int)]
        [InlineData(HashType.String)]
        [InlineData(HashType.StrLen)]
        public void Dictionary_Empty_GetKey(HashType hashType)
        {
            var d = Init<int>();
            Action action = () => d.Get(new TestKey(0, hashType));
            action.Should().Throw<KeyNotExists>();
        }

        [Theory]
        [InlineData(HashType.Int)]
        [InlineData(HashType.String)]
        [InlineData(HashType.StrLen)]
        public void Dictionary_NotEmpty_GetNotExistsKey(HashType hashType)
        {
            var d = Init<int>();
            for (int i = 0; i < 1000; i++)
            {
                d.Add(new TestKey(i, hashType), i);
            }
            Action action = () => d.Get(new TestKey(1000, hashType));
            action.Should().Throw<KeyNotExists>();
        }

        [Theory]
        [InlineData(HashType.Int)]
        [InlineData(HashType.String)]
        [InlineData(HashType.StrLen)]
        public void Dictionary_AddKeys_GetKeys(HashType hashType)
        {
            var d = Init<int>();
            for (int i = 0; i < 1000; i++)
            {
                d.Add(new TestKey(i, hashType), i);
            }
            for (int i = 0; i < 1000; i++)
            {
                d.Get(new TestKey(i, hashType)).Should().Be(i);
            }
        }

        [Theory]
        [InlineData(HashType.Int)]
        [InlineData(HashType.String)]
        [InlineData(HashType.StrLen)]
        public void Dictionary_Foreach(HashType hashType)
        {
            var d = Init<int>();
            for (int i = -1000; i < 1000; i++)
            {
                d.Add(new TestKey(i, hashType), i);
            }
            var items = d.OrderBy(x => x.Key);
            var j = -1000;
            foreach (var item in items)
            {
                item.Key.InternalKey.Should().Be(j);
                item.Value.Should().Be(j);
                j++;
            }
        }

        [Theory]
        [InlineData(HashType.Int)]
        [InlineData(HashType.String)]
        [InlineData(HashType.StrLen)]
        public void Dictionary_Foreach_Invalidation(HashType hashType)
        {
            var d = Init<int>();

            d.Add(new TestKey(1, hashType), 1);
            Action a = () =>
            {
                foreach (var item in d)
                {
                    d.Add(new TestKey(2, hashType), 2);
                }
            };
            var ex = Record.Exception(a);
            ex.Should().NotBeNull().And.BeOfType<InvalidOperationException>();
        }
    }
}

