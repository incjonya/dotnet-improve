using System.Collections.Generic;

namespace Common
{
    public interface IMyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public void Add(TKey key, TValue value);

        public TValue Get(TKey key);

        public int Count { get; }
    }
}
