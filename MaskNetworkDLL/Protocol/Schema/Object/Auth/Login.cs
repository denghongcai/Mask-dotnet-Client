// automatically generated, do not modify

namespace MaskGame.Protocol.Schema.Object.Auth
{

using System;
using FlatBuffers;

public sealed class Login : Table {
  public static Login GetRootAsLogin(ByteBuffer _bb) { return GetRootAsLogin(_bb, new Login()); }
  public static Login GetRootAsLogin(ByteBuffer _bb, Login obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Login __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string Username { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetUsernameBytes() { return __vector_as_arraysegment(4); }
  public string Password { get { int o = __offset(6); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetPasswordBytes() { return __vector_as_arraysegment(6); }

  public static Offset<Login> CreateLogin(FlatBufferBuilder builder,
      StringOffset usernameOffset = default(StringOffset),
      StringOffset passwordOffset = default(StringOffset)) {
    builder.StartObject(2);
    Login.AddPassword(builder, passwordOffset);
    Login.AddUsername(builder, usernameOffset);
    return Login.EndLogin(builder);
  }

  public static void StartLogin(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddUsername(FlatBufferBuilder builder, StringOffset usernameOffset) { builder.AddOffset(0, usernameOffset.Value, 0); }
  public static void AddPassword(FlatBufferBuilder builder, StringOffset passwordOffset) { builder.AddOffset(1, passwordOffset.Value, 0); }
  public static Offset<Login> EndLogin(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Login>(o);
  }
  public static void FinishLoginBuffer(FlatBufferBuilder builder, Offset<Login> offset) { builder.Finish(offset.Value); }
};


}
