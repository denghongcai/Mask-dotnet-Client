using FlatBuffers;
using MaskGame.Protocol.Schema.Object;

namespace MaskGame.RPC.Remote
{
    public class Auth
    {
        public static Error Login(string username, string password)
        {
            var builder = new FlatBufferBuilder(1);
            var usernameOffset = builder.CreateString(username);
            var passwordOffset = builder.CreateString(password);
            Protocol.Schema.Object.Auth.Login.StartLogin(builder);
            Protocol.Schema.Object.Auth.Login.AddUsername(builder, usernameOffset);
            Protocol.Schema.Object.Auth.Login.AddPassword(builder, passwordOffset);
            var request = Protocol.Schema.Object.Auth.Login.EndLogin(builder);
            builder.Finish(request.Value);

            var buf = builder.SizedByteArray();

            var meta = Wrapper.GetInstance().Call("auth.Login", buf);

            return null; // TODO
            return Error.GetRootAsError(new ByteBuffer(meta.Ret));
        }
    }
}
