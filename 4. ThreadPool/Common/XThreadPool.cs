using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Common
{
    public class XThreadPool
    {
        public event EventHandler<WorkResult> ActionCompleted;

        private readonly Thread _worker;

        private readonly BlockingCollection<WorkItem> _concurrentQueue = new BlockingCollection<WorkItem>();

        private readonly CancellationToken _ct;
        private readonly CancellationTokenSource _cts;

        private class WorkItem
        {
            public string Id { get; set; }
            public Action Work { get; set; } 
        }

        public XThreadPool() {
            _cts = new CancellationTokenSource();
            _ct = _cts.Token;
            _worker = new Thread(() => { StarWorkLoop(); });
            _worker.Start();
        }

        public int CurrentWorkCount => _concurrentQueue.Count;

        private void StarWorkLoop()
        {
            foreach (var workItem in _concurrentQueue.GetConsumingEnumerable(_ct))
            {
                try
                {
                    workItem.Work();
                    OnWorkCompleted(new WorkResult { WorkId = workItem.Id, Success = true });
                }
                catch (Exception ex)
                {
                    OnWorkCompleted(new WorkResult { WorkId = workItem.Id, Success = false, Error = ex });
                }
            }
        }

        private void OnWorkCompleted(WorkResult workResult)
        {
            ActionCompleted?.Invoke(this, workResult);
        }

        public void WaitForShutdown()
        {
            _concurrentQueue.CompleteAdding();
            _cts.Cancel();
            _worker.Join();
        }

        public string QueueWork(Action workItem)
        {
            var work = new WorkItem { Id = Guid.NewGuid().ToString(), Work = workItem } ;
            _concurrentQueue.Add(work);
            return work.Id;
        }
    }

    public class WorkResult
    {
        public string WorkId { get; set; }

        public bool Success { get; set; }

        public Exception Error { get; set; }
    }
}
