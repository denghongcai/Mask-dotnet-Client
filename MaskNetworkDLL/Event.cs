using System;
using MaskGame.Protocol;
using System.Linq;
using FlatBuffers;
using MaskGame.Queue;

namespace MaskGame
{
    public class EventArgs
    {
        public EventArgs(string fullName, object payload)
        {
            FullName = fullName;
            Payload = payload;
        }

        public EventArgs(string fullName, string shortid, object payload)
        {
            ShortId = shortid;
            FullName = fullName;
            Payload = payload;
        }

        public string ShortId { get; private set; }
        public string FullName { get; private set; } // FQN
        public string Name
        {
            get
            {
                return FullName.Split('.').Last();
            }
        }
        public object Payload { get; private set; }
    }

    public class EventWatcher
    {
        public readonly static EventWatcher instance = new EventWatcher();

        public static EventWatcher GetInstance()
        {
            return instance;
        }

        public delegate void EventHandlerDelegate(EventArgs eventArgs);

        public EventHandlerDelegate EventHandler = null;

        public void Loop()
        {
            while (EventQueue.GetInstance().Count != 0)
            {
                var ev = EventQueue.GetInstance().Dequeue();
                if(EventHandler != null)
                {
                    EventHandler(ev); 
                }
            }
        }
    }

    public class Event
    {
        public static EventArgs GetArgsFromPacket(Packet packet)
        {
            /*
            Type type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => t.IsClass && t.FullName == packet.Payload.FullName)
                .Single();
            object payload = type.GetMethod("GetRootAs" + type.Name, new Type[] { typeof(ByteBuffer)}).Invoke(null, new object[]{ new ByteBuffer(packet.Payload.Data)});
            */
            object payload = packet.Payload.Data;
            return new EventArgs(packet.Payload.FullName, packet.ShortId, payload);
        }

        public static void PacketHandler(Packet packet)
        {
            EventArgs eventArgs = Event.GetArgsFromPacket(packet);
            EventQueue.GetInstance().Enqueue(eventArgs);
        }
    }
}
