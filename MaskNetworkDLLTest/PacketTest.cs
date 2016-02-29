using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaskGame.Protocol;
using System.Linq;
using System.Diagnostics;

namespace MaskNetworkDLLTest
{
    [TestClass]
    public class PacketTest
    {
        [TestMethod]
        public void TestPackUnpack()
        {
            var packet = new Packet(new Payload(Payload.Types.RAW, "test", new byte[2] { 0x00, 0x11 }));
            var unpacket = new Packet(packet.PDU);
            Assert.AreEqual(packet.Payload.FullName, unpacket.Payload.FullName);
            Assert.AreEqual(packet.Payload.Type, unpacket.Payload.Type);
            CollectionAssert.AreEqual(packet.Payload.Data, unpacket.Payload.Data);
        }
    }
}
