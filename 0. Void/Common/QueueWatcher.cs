using System;
using Serilog;

namespace Common
{
    public abstract class QueueWatcher<T> : IDisposable
    {
        private XQueue<T> _queue;

        protected QueueWatcher(XQueue<T> queue)
        {
            _queue = queue;
            queue.ObjectEnqueued += Queue_ObjectEnqueued;
            queue.ObjectDequeued += Queue_ObjectDequeued;

            ProcessMessage($"{this} watcher subscribed to queue");
        }

        protected abstract void ProcessMessage(string msg);

        private void Queue_ObjectDequeued(XQueue<T> obj)
        {
            ProcessMessage($"Value removed from queue, Queue size = {obj.Count}");
        }

        private void Queue_ObjectEnqueued(XQueue<T> obj)
        {
            ProcessMessage($"Value added to queue. Queue size = {obj.Count}");
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _queue.ObjectEnqueued -= Queue_ObjectEnqueued;
                    _queue.ObjectDequeued -= Queue_ObjectDequeued;
                    ProcessMessage($"{this} watcher unsubscribed from queue");
                }

                disposedValue = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

    public class ConsoleQueueWatcher<T> : QueueWatcher<T>
    {
        public ConsoleQueueWatcher(XQueue<T> queue) : base(queue)
        {
        }

        protected override void ProcessMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }

    public class LogQueueWatcher<T> : QueueWatcher<T>
    {
        static LogQueueWatcher()
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs\\queue-watcher.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        }

        public LogQueueWatcher(XQueue<T> queue) : base(queue)
        {
            
        }

        protected override void ProcessMessage(string msg)
        {
            Log.Information(msg);
        }
    }
}
