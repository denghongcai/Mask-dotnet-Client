using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaskGame.RPC
{
    public class Client
    {
        public delegate void CallAsyncDelegate(Meta meta);

        private readonly static Client instance = new Client();

        public static Client GetInstance()
        {
            return instance;
        }

        public byte[] CallAsync(string methodName, byte[] @params)
        {

            return new byte[1];
        }
    }
}
