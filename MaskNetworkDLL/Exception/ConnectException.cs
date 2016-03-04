using System;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace MaskGame.Exception
{
    public class ConnectException : SocketException, ISerializable
    {
        public ConnectException() { }

        public virtual object ActualValue { get; }

        public override string Message { get; }
    }
}
