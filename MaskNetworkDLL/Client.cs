using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using UnityEngine;
using MaskGame.Queue;
using MaskGame.Exception;
using MaskGame.Protocol;

namespace MaskGame
{
    public class Client
    {
        public delegate void EventHandlerDelegate(EventArgs eventArgs);

        public EventHandlerDelegate EventHandler = null;

        public delegate void ReadPacketHandlerDelegate(Packet packet);

        public ReadPacketHandlerDelegate ReadPacketHandler = null;

        private string hostName;

        private int port = 1430;

        private volatile bool shouldStop = false;

        private volatile Socket socket;

        private object socketLock = new object();

        private BlockingQueue<Packet> writeQueue = new BlockingQueue<Packet>();

        private volatile bool disconnected = false;

        public Client(string hostname)
        {
            this.hostName = hostname;
        }

        public Client(string hostname, int port)
        {
            this.hostName = hostname;
            this.port = port;
        }

        private void InitEventHandler()
        {
            ReadPacketHandler = (packet) =>
            {
                EventArgs eventArgs = Event.GetArgsFromPacket(packet);
                if (EventHandler != null)
                {
                    EventHandler(eventArgs);
                }
            };
        }

        public void Connect()
        {
            if (hostName == null || disconnected == true) return;

            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socket.Connect(remoteEP);
                    Debug.Log("socket connected to " + hostName + ":" + port);
                }
                catch (ArgumentNullException ane)
                {
                    Debug.LogError("ArgumentNullException : " + ane.ToString());
                }
                catch (SocketException se)
                {
                    Debug.LogError("SocketException : " + se.ToString());
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Unexpected exception : " + e.ToString());
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        public void Reconnect()
        {
            disconnected = false;
            Connect();
        }

        public void BeginReadPacket()
        {
            ParameterizedThreadStart parStart = new ParameterizedThreadStart(ReadWorker);
            Thread readWorkerThread = new Thread(parStart);
            readWorkerThread.IsBackground = true;
            readWorkerThread.Start(ReadPacketHandler);
        }

        public void BeginWritePacket()
        {
            Thread writeWorkerThread = new Thread(WriteWorker);
            writeWorkerThread.IsBackground = true;
            writeWorkerThread.Start(writeWorkerThread);
        }

        private void ReadWorker(object obj)
        {
            Debug.Log("read worker started");
            var byteStream = new MemoryStream();
            while (!shouldStop)
            {
                try
                {
                    var bytes = Read();
                    var writer = new BinaryWriter(byteStream);
                    writer.Write(bytes);
                    writer.Flush();
                    try
                    {
                        var packet = new Packet(byteStream.ToArray(), true);
                        packet.Unpack();
                        if (obj != null)
                        {
                            ((ReadPacketHandlerDelegate)obj)(packet);
                        }
                        byteStream = new MemoryStream();
                    }
                    catch (ArgumentLengthNotSatisfiedException)
                    { }
                    catch (System.Exception e)
                    { /* unpack unexpected exception */
                        Debug.LogWarning(e.ToString());
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Read exception : " + e.ToString());
                }
            }
            Debug.Log("read worker terminated");
        }

        private void WriteWorker()
        {
            Debug.Log("write worker started");
            while (!shouldStop)
            {
                try
                {
                    write(writeQueue.Dequeue().PDU);
                }
                catch (InvalidOperationException ie)
                {
                    Debug.LogError("Write exception : " + ie.ToString());
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Write exception : " + e.ToString());
                }
            }
            Debug.Log("write worker terminated");
        }

        private Stream GetStream()
        {
            lock (socketLock)
            {
                if (socket == null || !socket.Connected)
                {
                    Close();
                    Reconnect();
                }
                return new NetworkStream(socket);
            }
        }

        public void Write(Packet packet)
        {
            writeQueue.Enqueue(packet);
        }

        private void write(byte[] bytes)
        {
            var stream = GetStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }

        private byte[] Read()
        {
            var stream = GetStream();
            var reader = new BinaryReader(stream);

            return reader.ReadBytes(1);
        }

        private void Close()
        {
            lock (socketLock)
            {
                writeQueue.Clear();
                if (socket != null)
                {
                    disconnected = true;
                    try
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                    }
                    catch (System.Exception)
                    { /* ignore close exception */ }
                    socket = null;
                }
            }
        }

        public void End()
        {
            writeQueue.Stop();
            shouldStop = true;
            Close();
        }
    }
}
