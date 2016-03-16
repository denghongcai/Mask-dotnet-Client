using FlatBuffers;
using MaskGame.Protocol.Schema.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MaskGame.RPC.Remote
{
    public class Player
    {
        private readonly static Player instance = new Player();

        public static Player GetInstance()
        {
            return instance;
        }

        private string id;
        private string scene;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Scene
        {
            get { return scene; }
        }

        public void AddToScene(string scene)
        {
            Wrapper.GetInstance().Call("scene." + scene + ".AddPlayer");
            this.scene = scene;
        }

        /// <summary>
        /// move player
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="jump"></param>
        public void Move(Transform transform, bool jump = false)
        {
            var builder = new FlatBufferBuilder(1);
            var direction = transform.rotation;
            var pos = transform.position;
            var id = builder.CreateString(this.id);

            Protocol.Schema.Object.Player.StartPlayer(builder);

            Protocol.Schema.Object.Player.AddPos(builder, Vec3.CreateVec3(builder, pos.x, pos.y, pos.z));
            Protocol.Schema.Object.Player.AddDirection(builder, Vec3.CreateVec3(builder, direction.x, direction.y, direction.z));

            Protocol.Schema.Object.Player.AddId(builder, id);
            Protocol.Schema.Object.Player.AddJump(builder, jump);
            var player = Protocol.Schema.Object.Player.EndPlayer(builder);
            Protocol.Schema.Object.Player.FinishPlayerBuffer(builder, player);

            Wrapper.GetInstance().Call("scene." + scene + ".movePlayer", builder.SizedByteArray());
        }

        public void Remove()
        {
            Wrapper.GetInstance().Call("scene." + scene + ".RemovePlayer");
        }

        public Task<Meta> Speak(byte[] data)
        {
            var builder = new FlatBufferBuilder(1);
            var id = builder.CreateString(this.id);
            var dataOffset = VoiceSegment.CreateDataVector(builder, data);
            VoiceSegment.StartVoiceSegment(builder);
            VoiceSegment.AddPlayerid(builder, id);
            VoiceSegment.AddData(builder, dataOffset);
            var voiceSegment = VoiceSegment.EndVoiceSegment(builder);
            VoiceSegment.FinishVoiceSegmentBuffer(builder, voiceSegment);

            return Wrapper.GetInstance().CallAsync("scene." + scene + ".Speak", builder.SizedByteArray());
        }
    }
}
