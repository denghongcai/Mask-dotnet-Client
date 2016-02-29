using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MaskGame.Protocol
{
    public class Packet
    {
        const uint MAX_PACKET_LENGTH = 1 << 20;
        const int MAX_SHORTID_LENGTH = 16; // including '\0' 

        private BitArray ext = new BitArray(new byte[2] { 0x00, 0x00 }); // protocol ext

        public int Version
        {
            get
            {
                return Convert.ToInt32(ext[0]);
            }
            set
            {
                ext[0] = Convert.ToBoolean(value);
            }
        }

        public UInt32 Timestamp; // seconds
        private string shortId = "";
        public string ShortId
        {
            get
            {
                return shortId;
            }
            set
            {
                if(value.Length > MAX_SHORTID_LENGTH - 1)
                {
                    throw new ArgumentException();
                }
                shortId = value;
            }
        }// short id

        public Payload Payload;

        public byte[] PDU;

        /// <summary>
        /// construct Packet and pack
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="Data"></param>
        public Packet(Payload payload)
        {
            Timestamp = Util.Date.UtcNow().Seconds;
            Payload = payload;
            Pack();
        }

        /// <summary>
        /// construct Packet and unpack
        /// </summary>
        /// <param name="pdu"></param>
        public Packet(byte[] pdu)
        {
            this.PDU = pdu;
            Unpack();
        }
        
        /// <summary>
        /// construct Packet but unpack later
        /// </summary>
        /// <param name="pdu"></param>
        /// <param name="partial"></param>
        public Packet(byte[] pdu, bool partial)
        {
            this.PDU = pdu;
        }

        /// <summary>
        /// get instance size of Packet
        /// </summary>
        public int Size
        {
            get
            {
                return sizeof(uint) + 2 + sizeof(uint) + MAX_SHORTID_LENGTH + Payload.Length;
            }
        }

        public void Pack()
        {
            var stream = new MemoryStream();
            var lengthBytes = Util.Bytes.GetBEBytesFromNumber((uint)Payload.Length);
            var extBytes = Util.Bytes.BitArrayToBytes(ext);
            var timestampBytes = Util.Bytes.GetBEBytesFromNumber(Timestamp);
            var paddedShortidBytes = Util.Bytes.PaddingBytes(Encoding.ASCII.GetBytes(ShortId), MAX_SHORTID_LENGTH);

            using(var writer = new BinaryWriter(stream))
            {
                writer.Write(lengthBytes);
                writer.Write(extBytes);
                writer.Write(timestampBytes);
                writer.Write(paddedShortidBytes);
                writer.Write(Payload.ToBytes());

                writer.Flush();
            }

            PDU = stream.ToArray();
        }

        public void Unpack()
        {
            if(PDU.Length < 4)
            {
                throw new Exception.ArgumentLengthNotSatisfiedException();
            }
            var length = Util.Bytes.GetUInt32FromBEBytes(PDU);
            if(length > MAX_PACKET_LENGTH)
            {
                throw new ArgumentOutOfRangeException();
            }
            if(PDU.Length < (sizeof(uint) + 2 + sizeof(uint) + MAX_SHORTID_LENGTH + length))
            {
                throw new Exception.ArgumentLengthNotSatisfiedException();
            }
            var stream = new MemoryStream(PDU);
            stream.Seek(sizeof(uint), 0);
            using(var reader = new BinaryReader(stream))
            {
                ext = new BitArray(reader.ReadBytes(2));
                Timestamp = Util.Bytes.GetUInt32FromBEBytes(reader.ReadBytes(sizeof(UInt32)));
                ShortId = Util.Bytes.ReadStringUntil(reader.ReadBytes(MAX_SHORTID_LENGTH), '\0');
                Payload = Payload.NewFromBytes(reader.ReadBytes((int)length));
            }
        }
    }

    public class Payload
    {
        const int MAX_FULLNAME_LENGTH = 64;

        public enum Types { RAW, RPC};

        public Types Type;

        private string fullName;
        public string FullName {
            get
            {
                return fullName.Trim('\0');
            }
            set
            {
                if(value.Length > MAX_FULLNAME_LENGTH - 1)
                {
                    throw new ArgumentException();
                }
                fullName = value + '\0';
            }
        }
        public byte[] Data;
        
        public Payload(Types type, string fullName, byte[] data)
        {
            Type = type;
            FullName = fullName;
            Data = data; 
        }

        public byte[] ToBytes()
        {
            var typeBytes = new byte[1] { (byte)Type };
            var fullNameBytes = Encoding.ASCII.GetBytes(fullName);
            return typeBytes.Concat(fullNameBytes).Concat(Data).ToArray();
        }

        public static Payload NewFromBytes(byte[] bytes)
        {
            var type = Util.Bytes.GetUInt8FromBEBytes(bytes);
            var fullName = Util.Bytes.ReadStringUntil(bytes.Skip(1).ToArray(), '\0');
            var data = bytes.Skip(1 + fullName.Length + 1).ToArray();
            return new Payload((Types)type, fullName, data);
        }

        public int Length
        {
            get
            {
                return 1 + fullName.Length + Data.Length;
            }
        }
    }
}
