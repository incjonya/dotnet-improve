using System;

namespace Common.Tests
{
    public enum HashType
    {
        Int, String, StrLen
    }


    public struct TestKey : IComparable
    {
        private readonly int _key;

        public int InternalKey => _key;

        private HashType _hashType;

        public TestKey(int key) : this (key, HashType.Int)
        {
        }

        public TestKey(int key, HashType hashType)
        {
            _key = key;
            _hashType = hashType;
        }

        public override int GetHashCode() => _hashType switch
        {
            HashType.Int => _key,
            HashType.String => _key.ToString().GetHashCode(),
            HashType.StrLen => _key.ToString().Length,
            _ => throw new ArgumentException("Not correct hash type")
        };

        public override bool Equals(object obj)
        {
            if (obj is TestKey k)
            {
                return k._key == _key;
            }
            return false;
        }

        public int CompareTo(object obj)
        {
            if (obj is TestKey k)
            {
                return _key.CompareTo(k._key);
            }
            throw new ArgumentException();
        }
    }
}
