using MaskGame.Protocol;
using System.Threading.Tasks;

namespace MaskGame.RPC
{
    public class Client
    {
        //public delegate void CallAsyncDelegate(Meta meta);

        private readonly static Client instance = new Client();

        public static Client GetInstance()
        {
            return instance;
        }

        public Task<Meta> CallAsync(string methodName, byte[] @params)
        {
            var meta = new Meta(methodName, @params);
            var payload = new Payload(Payload.Types.RPC, methodName, @params);
            var packet = new Packet(payload);
            Buffer.GetInstance().Set(packet.ShortId, meta);

            MaskGame.Client.GetInstance().Write(packet);

            var t = new Task<Meta>(() =>
            {
                meta.Lock.WaitOne(1000);
                return meta;
            });
            t.Start();
            
            return t;
        }

        public Task<Meta> CallAsync(string methodName)
        {
            return CallAsync(methodName, new byte[0]);
        }

        public Meta Call(string methodName, byte[] @params)
        {
            var t = CallAsync(methodName, @params);
            t.Wait();
            return t.Result;
        }

        public Meta Call(string methodName)
        {
            return Call(methodName, new byte[0]);
        }
    }
}
