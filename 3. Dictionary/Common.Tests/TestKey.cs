using System;

namespace Common.Tests
{
    public struct TestKey : IComparable
    {
        private int _key;

        public int InternalKey => _key;

        public TestKey(int key)
        {
            _key = key;
        }

        public override int GetHashCode() => _key;

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
