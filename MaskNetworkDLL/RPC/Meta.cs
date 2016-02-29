using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaskGame.RPC
{
    public class Meta
    {
        public string ShortId = Util.ShortId.NewString();
        public string MethodName { get; private set; }
        public byte[] Params { get; private set; }
        public byte[] Ret;
        public Client.CallAsyncDelegate cb;

        public Meta(string methodName, byte[] @params)
        {
            MethodName = methodName;
            Params = @params;
        }
    }
}
