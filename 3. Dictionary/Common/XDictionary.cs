using System;
using System.Collections;
using System.Collections.Generic;

namespace Common
{
    public class XDictionary<TKey, TValue> : IMyDictionary<TKey, TValue>
    {
        public struct Bucket
        {
            public KeyEntry Root { get; set; }
        }

        public class KeyEntry
        {
            public int CurrentIndex { get; set; } = -1;
            public TKey Key { get; set; }
            public KeyEntry Next { get; set; }
        }

        public const int DEFAULT_SIZE = 4;

        private TKey[] _keys;
        private TValue[] _values;
        
        private Bucket[] _buckets;

        private bool _inEnumeration;

        public XDictionary() : this(DEFAULT_SIZE) { }

        public XDictionary(int size)
        {
            _keys = new TKey[size];
            _values = new TValue[size];
            _buckets = new Bucket[size];
        }

        public int Count { get; private set; } = 0;

        public TValue Get(TKey key)
        {
            var keyEntry = FindKeyIndex(key);

            if (keyEntry == null)
                throw new KeyNotExists();

            if (keyEntry.Key.Equals(key))
                return _values[keyEntry.CurrentIndex];

            if (keyEntry.Next == null)
                throw new KeyNotExists();

            return _values[keyEntry.Next.CurrentIndex];
        }

        public void Add(TKey key, TValue value)
        {
            if (_inEnumeration)
                throw new InvalidOperationException("Collection was modified after the enumerator was instantiated");

            var keyEntry = FindKeyIndex(key);
            if (keyEntry != null && (keyEntry.Next != null || keyEntry.Key.Equals(key)))
                throw new SameKeyException();

            if (_keys.Length == Count)
            {
                EnsureCapacity();
                keyEntry = null; //нужно сбросить
            }
                

            _keys[Count] = key;
            _values[Count] = value;

            if (keyEntry != null)
            {
                keyEntry.Next = new KeyEntry { CurrentIndex = Count, Key = key, Next = null };
            }
            else
            {
                // размер поменялся, добавляем через новый поиск
                AddInternal(key, Count);
            }

            Count++;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            _inEnumeration = true;

            for (int i = 0; i < Count; i++)
                yield return new KeyValuePair<TKey, TValue>(_keys[i], _values[i]);

            _inEnumeration = false;
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private int HashInternal(TKey key) => Math.Abs(key.GetHashCode()) % _buckets.Length;

        private void AddInternal(TKey key, int keyIndex)
        {
            var idx = HashInternal(key);
            var keyEntry = new KeyEntry { CurrentIndex = keyIndex, Key = key, Next = null };
            if (_buckets[idx].Root == null)
            {
                _buckets[idx].Root = keyEntry;
            }
            else
            {
                var root = _buckets[idx].Root;
                while (root.Next != null) root = root.Next;
                root.Next = keyEntry;
            }
        }

        private KeyEntry FindKeyIndex(TKey key)
        {
            if (Count == 0)
                return null;

            var bucket = _buckets[HashInternal(key)];

            if (bucket.Root == null)
                return null;

            var root = bucket.Root;
            var previous = bucket.Root;
            while (root != null && !root.Key.Equals(key)) { previous = root; root = root.Next; }            

            return previous; // или ключ в previous.Next или previous.Next = null и нужно прицепить к нему
        }

        private void EnsureCapacity()
        {
            var tmpKeys = new TKey[_keys.Length * 2];
            var tmpValues = new TValue[_keys.Length * 2];

            Array.Copy(_keys, 0, tmpKeys, 0, _keys.Length);
            Array.Copy(_values, 0, tmpValues, 0, _keys.Length);

            _buckets = new Bucket[_keys.Length * 2];
            for (int i=0; i<_keys.Length; i++)
            {
                AddInternal(_keys[i], i);
            }

            _keys = tmpKeys;
            _values = tmpValues;
        }
    }
}
