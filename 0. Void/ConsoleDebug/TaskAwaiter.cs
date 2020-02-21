using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace ConsoleDebug
{
    public class TaskAwaiter : INotifyCompletion
    {
        private Action _continuation;

        public bool IsCompleted { get; private set; }

        public TaskAwaiter GetAwaiter() => this;

        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;
        }

        internal void SetCompleted()
        {
            IsCompleted = true;
            Interlocked.Exchange(ref _continuation, null)?.Invoke();
        }

        private int _i;

        public int GetResult()
        {
            IsCompleted = false;
            return _i++;
        }
    }
}
