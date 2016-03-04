using MaskGame.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace MaskGame.RPC
{
    public class Watcher
    {
        public static void PacketHandler(Packet packet)
        {
            try
            {
                var meta = Buffer.GetInstance().Get(packet.ShortId);
                meta.Ret = packet.Payload.Data;
                meta.Lock.Set();
            }
            catch(KeyNotFoundException ke)
            {
                Debug.LogWarning(ke);
            }
        }
    }
}
