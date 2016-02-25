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
            var packet = new Packet("test", new byte[] { 0x20, 0x20, 0x20, 0x20, 0x40, 0x50 });
            var unpacket = new Packet(packet.PDU);
            Assert.AreEqual<string>(packet.FullName, unpacket.FullName);
            CollectionAssert.AreEqual(packet.Data, unpacket.Data);
        }
    }
}
