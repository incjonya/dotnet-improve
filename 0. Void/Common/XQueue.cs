using System;
using System.Linq;

namespace Common
{
    public class XQueue<T>
    {
        class XNode
        {
            public XNode(T value)
            {
                Value = value;
            }

            public XNode Next { get; set; }
            public XNode Previous { get; set; }

            public T Value { get; private set; }
        }

        public event Action<XQueue<T>> ObjectEnqueued;
        public event Action<XQueue<T>> ObjectDequeued;

        XNode _first, _last;
        private readonly object _l = new object();

        public XQueue()
        {
            _first = _last = null;
            Count = 0;
        }

        public void Enqueue(T item)
        {
            var node = new XNode(item);

            lock (_l)
            {
                if (_first == null && _last == null)
                {
                    _first = node;
                }
                else
                {
                    _last.Next = node;
                    node.Previous = _last;
                }

                _last = node;
                Count++;

                OnObjectEnqueued();
            }
        }

        public T Dequeue()
        {
            if (Count == 0)
                throw new ArgumentException("The queue is empty");

            lock (_l)
            {
                T item = _first.Value;

                if (Count == 1)
                {
                    _first = _last = null;
                }
                else
                {
                    _first = _first.Next;
                    _first.Previous = null;
                }

                Count--;

                OnObjectDequeued();

                return item;
            }
        }

        public int Count { get; private set; }
        public string WatchersInfo
        {
            get
            {
                var handlerEnq = ObjectEnqueued;
                var handlerDeq = ObjectDequeued;

                return (handlerEnq == null ? "No Enq watcher" : $"Enq watcher(s) exists (target(s): {string.Join(",", handlerEnq.GetInvocationList().Select(x => x.Target))})") + "/"
                     + (handlerDeq == null ? "No Deq watcher" : $"Deq watcher(s) exists (target(s): {string.Join(",", handlerDeq.GetInvocationList().Select(x => x.Target))})");
            }
        }



        private void OnObjectEnqueued()
        {
            ObjectEnqueued?.Invoke(this);
        }

        private void OnObjectDequeued()
        {
            ObjectDequeued?.Invoke(this);
        }
    }
}
