using System;
using System.Collections;
using System.Collections.Generic;

namespace Common
{
    public class XDictionary<TKey, TValue> : IMyDictionary<TKey, TValue>
    {
        public const int DEFAULT_SIZE = 4;

        private TKey[] _keys;
        private TValue[] _values;
        private int[] _hashes;

        private bool _inEnumeration;

        public XDictionary() : this(DEFAULT_SIZE) { }

        public XDictionary(int size)
        {
            _keys = new TKey[size];
            _values = new TValue[size];
            _hashes = new int[size];
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

            _hashes[Count] = key.GetHashCode();
            _keys[Count] = key;
            _values[Count] = value;

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

        private int FindKeyIndex(TKey key)
        {
            if (Count == 0)
                return -1;
            
            for (int i = 0; i < Count; i++)
            {
                if (key.GetHashCode() == _hashes[i] && key.Equals(_keys[i]))
                {
                    return i;                    
                }
            }

            return -1;
        }

        private void EnsureCapacity()
        {
            var tmpKeys = new TKey[_keys.Length * 2];
            var tmpValues = new TValue[_keys.Length * 2];
            var tmpHashes = new int[_hashes.Length * 2];

            Array.Copy(_keys, 0, tmpKeys, 0, _keys.Length);
            Array.Copy(_values, 0, tmpValues, 0, _keys.Length);
            Array.Copy(_hashes, 0, tmpHashes, 0, _keys.Length);

            _keys = tmpKeys;
            _values = tmpValues;
            _hashes = tmpHashes;
        }
    }
}
