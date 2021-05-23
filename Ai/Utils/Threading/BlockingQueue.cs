using System;
using System.Collections.Generic;
using System.Threading;

namespace MRL.SSL.Ai.Utils.Threading
{

    public sealed class BlockingQueue<T> : IDisposable
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private Semaphore _pool = new Semaphore(0, int.MaxValue);
        private readonly object _lock = new object();


        public void Enqueue(T item)
        {
            lock (_lock)
            {
                _queue.Enqueue(item);
                _pool.Release();
            }
        }

        public T Dequeue()
        {
            _pool.WaitOne();
            lock (_lock)
            {
                return _queue.Dequeue();
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            var pool = Interlocked.Exchange(ref _pool, null);
            if (pool != null) pool.Dispose();

        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public T[] ToArray()
        {
            lock (_lock)
            {
                return _queue.ToArray();
            }
        }
    }
}