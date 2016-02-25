using System.Collections.Generic;
using System.Linq;

namespace MaskGame.Queue
{
    public sealed class EventQueue
    {
        private readonly static EventQueue instance = new EventQueue();

        private Queue<object> eventQueue = new Queue<object>();

        public static EventQueue GetInstance()
        {
            return instance;
        }

        public void Enqueue(object obj)
        {
            eventQueue.Enqueue(obj); 
        }

        public object Dequeue()
        {
            return eventQueue.Dequeue(); 
        }

        public int Count
        {
            get
            {
                return eventQueue.Count();
            }
        }
    }
}
