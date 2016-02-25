using System;
using MaskGame.Protocol;
using System.Linq;
using System.Reflection;

namespace MaskGame
{
    public class EventArgs
    {
        public EventArgs(string fullName, string name, object payload)
        {
            this.FullName = fullName;
            this.Name = name;
            this.Payload = payload;
        }
        public string FullName { get; private set; }
        public string Name { get; private set; } // FQDN
        public object Payload { get; private set; }
    }

    public class Event
    {
        public static EventArgs GetArgsFromPacket(Packet packet)
        {
            Type type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(t => t.GetTypes())
                .Where(t => t.IsClass && t.FullName == packet.FullName)
                .Single();
            object payload = type.GetMethod("GetRootAs" + type.Name, BindingFlags.Public | BindingFlags.Static).Invoke(null, new object[]{ packet.Data});
            return new EventArgs(type.FullName, type.Name, payload);
        }
    }
}
