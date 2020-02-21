using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Common.Tests
{
    public class BaseTests
    {
        public static IMyDictionary<TestKey, TValue> Init<TValue>() => new XDictionary<TestKey, TValue>();

        [Fact]
        public void Dictionary_Empty_AddKey()
        {
            var d = Init<int>();
            for (int i = 0; i < 100000; i++)
            {
                d.Add(new TestKey(i), i);
            }
            d.Count.Should().Be(100000);
        }

        [Fact]
        public void Dictionary_AddDuplicateKey()
        {
            var d = Init<int>();
            Action action = () =>
            {
                d.Add(new TestKey(42), 1);
                d.Add(new TestKey(42), 2);
            };
            action.Should().Throw<SameKeyException>();
        }

        [Fact]
        public void Dictionary_Empty_GetKey()
        {
            var d = Init<int>();
            Action action = () => d.Get(new TestKey(0));
            action.Should().Throw<KeyNotExists>();
        }

        [Fact]
        public void Dictionary_NotEmpty_GetNotExistsKey()
        {
            var d = Init<int>();
            for (int i = 0; i < 100000; i++)
            {
                d.Add(new TestKey(i), i);
            }
            Action action = () => d.Get(new TestKey(100000));
            action.Should().Throw<KeyNotExists>();
        }

        [Fact]
        public void Dictionary_AddKeys_GetKeys()
        {
            var d = Init<int>();
            for (int i = 0; i < 100000; i++)
            {
                d.Add(new TestKey(i), i);
            }
            for (int i = 0; i < 100000; i++)
            {
                d.Get(new TestKey(i)).Should().Be(i);
            }
        }

        [Fact]
        public void Dictionary_Foreach()
        {
            var d = Init<int>();
            for (int i = -100000; i < 100000; i++)
            {
                d.Add(new TestKey(i), i);
            }
            var items = d.OrderBy(x => x.Key);
            var j = -100000;
            foreach (var item in items)
            {
                item.Key.InternalKey.Should().Be(j);
                item.Value.Should().Be(j);
                j++;
            }
        }

        [Fact]
        public void Dictionary_Foreach_Small()
        {
            var d = Init<int>();
            for (int i = -10; i < 10; i++)
            {
                d.Add(new TestKey(i), i);
            }
            var items = d.OrderBy(x => x.Key);
            var j = -10;
            foreach (var item in items)
            {
                item.Key.InternalKey.Should().Be(j);
                item.Value.Should().Be(j);
                j++;
            }
        }

        [Fact]
        public void Dictionary_ModifyEnum_ThrowInvalidOperation()
        {
            var d = Init<int>();
            for (int i = -10; i < 10; i++)
            {
                d.Add(new TestKey(i), i);
            }

            Action action = () => {
                foreach (var item in d)
                {
                    d.Add(new TestKey(666), 777);
                }
            };

            action.Should().Throw<InvalidOperationException>().WithMessage("Collection was modified after the enumerator was instantiated");
        }

        [Fact]
        public void Dictionary_Foreach_CheckPerformance()
        {
            var d = new Dictionary<TestKey, int>();
            for (int i = -100000; i < 100000; i++)
            {
                d.Add(new TestKey(i), i);
            }
            var items = d.OrderBy(x => x.Key);
            var j = -100000;
            foreach (var item in items)
            {
                item.Key.InternalKey.Should().Be(j);
                item.Value.Should().Be(j);
                j++;
            }
        }
    }
}
