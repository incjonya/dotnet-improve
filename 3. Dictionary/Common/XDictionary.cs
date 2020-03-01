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
            public int CurrentIndex { get; set; }
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
            int index = FindKeyIndex(key);

            if (index < 0)
                throw new KeyNotExists();

            return _values[index];
        }

        public void Add(TKey key, TValue value)
        {
            if (_inEnumeration)
                throw new InvalidOperationException("Collection was modified after the enumerator was instantiated");

            int index = FindKeyIndex(key);
            if (index >= 0)
                throw new SameKeyException();

            if (_keys.Length == Count)
                EnsureCapacity();

            _keys[Count] = key;
            _values[Count] = value;

            AddInternal(key, Count);

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

        private int FindKeyIndex(TKey key)
        {
            if (Count == 0)
                return -1;

            var keyEntry = _buckets[HashInternal(key)];

            if (keyEntry.Root == null)
                return -1;

            var root = keyEntry.Root;
            while(root != null && !root.Key.Equals(key)) root = root.Next;

            return root == null ? -1 : root.CurrentIndex;
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
