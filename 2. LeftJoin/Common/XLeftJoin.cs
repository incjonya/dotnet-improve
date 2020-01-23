using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class XLeftJoin
    {
        public static IEnumerable<Item1> Get(IEnumerable<Item1> source, IEnumerable<Item2> join)
        {
            return new XEnumerable(source, join);
        }

        public static IEnumerable<Item1> GetYield(IEnumerable<Item1> source, IEnumerable<Item2> join)
        {
            foreach(var x in source)
            {
                var joined = new Item1 { ID = x.ID, Value = x.Value, Details = join.FirstOrDefault(x => x.ID == x.ID)?.Details };
                yield return joined;
            }
        }
    }

    public class XEnumerable : IEnumerable<Item1>
    {
        private readonly IEnumerator<Item1> _enumerator;

        public XEnumerable(IEnumerable<Item1> source, IEnumerable<Item2> join)
        {
            _enumerator = new XEnumerator(source, join);
        }

        public IEnumerator<Item1> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _enumerator;
        }
    }

    public class XEnumerator : IEnumerator<Item1>
    {
        private IEnumerable<Item2> _join;
        private IEnumerable<Item1> _source;
        private IEnumerator<Item1> _sourceEnumerator;
        private Item1 _current;

        public XEnumerator(IEnumerable<Item1> source, IEnumerable<Item2> join)
        {
            _join = join;
            _source = source;
        }

        private Item1 GetCurrent()
        {
            if (_current == null && _sourceEnumerator  != null && _sourceEnumerator.Current != null)
            {
                _current = new Item1 { ID = _sourceEnumerator.Current.ID, Value = _sourceEnumerator.Current.Value };
                _current.Details = _join.FirstOrDefault(x => x.ID == _current.ID)?.Details;
            }
            return _current;
        }

        public Item1 Current => GetCurrent();        

        object IEnumerator.Current => GetCurrent();

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _sourceEnumerator.Dispose();
                _sourceEnumerator = null;
            }
        }

        public bool MoveNext()
        {
            _current = null;
            if (_sourceEnumerator == null)
            {
                _sourceEnumerator = _source.GetEnumerator();
            }

            return _sourceEnumerator.MoveNext();
        }

        public void Reset()
        {
            _current = null;
            _sourceEnumerator.Reset();
        }
    }


    public class Item1
    {
        public int ID { get; set; }

        public int Value { get; set; }

        public string Details { get; set; }
    }

    public class Item2
    {
        public int ID { get; set; }

        public string Details { get; set; }
    }
}
