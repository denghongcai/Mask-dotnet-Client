using System;
using System.Linq;
using System.Text;

namespace MaskGame.Protocol
{
    public class Packet
    {
        const uint MAX_PACKET_LENGTH = 1 << 20;

        private uint length;
        public byte[] PDU;
        public string FullName; // fullname, including namespace and class name
        public byte[] Data;

        public Packet(string type, byte[]data)
        {
            this.FullName = type;
            this.Data = data;
            Pack();
        }

        public Packet(byte[] pdu)
        {
            this.PDU = pdu;
            Unpack();
        }

        public Packet(byte[] pdu, bool partial)
        {
            this.PDU = pdu;
        }

        public int Size
        {
            get
            {
                return sizeof(uint) + 8 + Data.Length;
            }
        }

        public void Pack()
        {
            byte[] bytes = new byte[sizeof(uint) + 8 + Data.Length];
            byte[] lengthBytes, typeBytes;
            length = (uint)Data.Length;
            lengthBytes = BitConverter.GetBytes(length);
            if(BitConverter.IsLittleEndian)
            {
                lengthBytes = lengthBytes.Reverse().ToArray();
            }
            typeBytes = Encoding.ASCII.GetBytes(FullName);
            byte[] paddedTypeBytes = new byte[8];
            Buffer.BlockCopy(typeBytes, 0, paddedTypeBytes, 0, typeBytes.Length);
            Buffer.BlockCopy(lengthBytes, 0, bytes, 0, sizeof(uint));
            Buffer.BlockCopy(paddedTypeBytes, 0, bytes, sizeof(uint), 8);
            Buffer.BlockCopy(Data, 0, bytes, sizeof(uint) + 8, Data.Length);
            PDU = bytes;
        }

        public void Unpack()
        {
            if(PDU.Length < 4)
            {
                throw new Exception.ArgumentLengthNotSatisfiedException();
            }
            byte[] lengthBytes = PDU.Take(sizeof(uint)).ToArray();
            if(BitConverter.IsLittleEndian)
            {
                lengthBytes = lengthBytes.Reverse().ToArray();
            }
            length = BitConverter.ToUInt32(lengthBytes, 0);
            if(length > MAX_PACKET_LENGTH)
            {
                throw new ArgumentOutOfRangeException();
            }
            if(PDU.Length < (sizeof(uint) + 8 + length))
            {
                throw new Exception.ArgumentLengthNotSatisfiedException();
            }
            FullName = Encoding.ASCII.GetString(PDU.Skip(sizeof(uint)).Take(8).ToArray()).Trim('\0');
            Data = PDU.Skip(sizeof(uint) + 8).ToArray();
        }
    }
}
