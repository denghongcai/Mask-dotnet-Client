using System.Collections.Generic;
using System.Threading;

namespace MaskGame.Queue
{
    class BlockingQueue<T>
    {
        private readonly Queue<T> queue = new Queue<T>();
        private volatile bool stopped;

        public bool Enqueue(T item)
        {
            if (stopped) return false;
            lock(queue)
            {
                if (stopped) return false;
                queue.Enqueue(item);
                Monitor.Pulse(queue);
                return true;
            }
        }

        public T Dequeue()
        {
            if (stopped) return default(T);
            lock(queue)
            {
                if (stopped) return default(T);
                while (queue.Count == 0)
                {
                    Monitor.Wait(queue);
                    if (stopped) return default(T);
                }
                return queue.Dequeue();
            }
        }

        public void Clear()
        {
            lock(queue)
            {
                queue.Clear();
            }
        }

        public void Stop()
        {
            if (stopped) return;
            lock(queue)
            {
                if (stopped) return;
                stopped = true;
                Monitor.PulseAll(queue);
            }
        }

        ~BlockingQueue()
        {
            Stop();
        }
    }
}
