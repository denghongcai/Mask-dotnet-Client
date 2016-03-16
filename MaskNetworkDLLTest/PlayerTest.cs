using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlatBuffers;
using UnityEngine;
using MaskGame.Protocol.Schema.Object;

namespace MaskGameTest
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void TestConstructPlayer()
        {
            var builder = new FlatBufferBuilder(1);
            var id = builder.CreateString("123");
            //var directVec3 = Vec3.CreateVec3(builder, 0, 0, 0);

            Player.StartPlayer(builder);

            Player.AddPos(builder, Vec3.CreateVec3(builder, 0, 0, 0));
            //Player.AddDirection(builder, directVec3);

            Player.AddId(builder, id);
            var player = Player.EndPlayer(builder);
            Player.FinishPlayerBuffer(builder, player);
        }
    }
}
