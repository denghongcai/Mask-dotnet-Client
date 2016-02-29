using System.Collections.Generic;
using System.Linq;

namespace MaskGame.Queue
{
    public sealed class EventQueue
    {
        private readonly static EventQueue instance = new EventQueue();

        private Queue<EventArgs> eventQueue = new Queue<EventArgs>();

        public static EventQueue GetInstance()
        {
            return instance;
        }

        public void Enqueue(EventArgs obj)
        {
            lock(eventQueue)
            {
                eventQueue.Enqueue(obj);
            }
        }

        public EventArgs Dequeue()
        {
            lock(eventQueue)
            {
                return eventQueue.Dequeue();
            }
        }

        public int Count
        {
            get
            {
                lock(eventQueue)
                {
                    return eventQueue.Count();
                }
            }
        }
    }
}
