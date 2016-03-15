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
        const int MAX_RETRY_TIMES = 5;

        private readonly static Client instance = new Client();

        public static Client GetInstance()
        {
            return instance;
        }

        public delegate void ReadPacketHandlerDelegate(Packet packet);

        public ReadPacketHandlerDelegate ReadPacketHandler = null;

        public delegate void AfterConnectHandlerDelegate();

        public AfterConnectHandlerDelegate AfterConnectHandler = null;

        private string hostName;

        private int port = 1430; // default port

        private bool shouldStop = false;

        private volatile Socket socket;

        private object socketLock = new object();

        private BlockingQueue<Packet> writeQueue = new BlockingQueue<Packet>();

        private bool disconnected = true;
        
        public bool Connected { get { return !disconnected;  } }

        private DateTime lastRetryTime = DateTime.UtcNow;

        private int connectRetryTimes = 0;

        public Client()
        {
            InitEventHandler();
        }

        private void InitEventHandler()
        {
            ReadPacketHandler = (packet) =>
            {
                switch(packet.Payload.Type)
                {
                    case Payload.Types.RAW:
                        Event.PacketHandler(packet);
                        break;
                    case Payload.Types.RPC:
                        RPC.Watcher.PacketHandler(packet);
                        break;
                }
            };
        }

        public void Connect()
        {
            if (hostName == null) return;
            Connect(hostName, port);
        }

        public void Connect(string hostName)
        {
            Connect(hostName, port);
        }

        public void Connect(string hostName, int port)
        {
            this.hostName = hostName;
            this.port = port;
            try
            {
                IPAddress ipAddress;
                if(!IPAddress.TryParse(hostName, out ipAddress))
                {
                    IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);
                    ipAddress = ipHostInfo.AddressList[0];
                }
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socket.Connect(remoteEP);
                    if(AfterConnectHandler != null)
                    {
                        AfterConnectHandler();
                    }
                    disconnected = false;
                    Debug.Log("socket connected to " + hostName + ":" + port);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Unexpected exception : " + e.ToString());
                    throw new ConnectException();
                }
            }
            catch (ConnectException ce)
            {
                throw ce;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        public void Reconnect()
        {
            if(disconnected == false)
            {
                if ((DateTime.UtcNow - lastRetryTime).Seconds > 2) Connect();
            }
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
                    var packet = writeQueue.Dequeue();
                    if(packet != null)
                    {
                        write(packet.PDU);
                    }
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
            disconnected = true;
            writeQueue.Stop();
            shouldStop = true;
            Close();
        }
    }
}
