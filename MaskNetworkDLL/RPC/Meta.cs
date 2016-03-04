using MaskGame.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MaskGame.RPC
{
    public class Meta
    {
        public string MethodName { get; private set; }
        public byte[] Params { get; private set; }
        public byte[] Ret;
        public readonly uint Timestamp = Util.Date.UtcNow().Seconds;
        public EventWaitHandle Lock = new EventWaitHandle(false, EventResetMode.ManualReset);

        public Meta(string methodName, byte[] @params)
        {
            MethodName = methodName;
            Params = @params;
        }
    }
}
