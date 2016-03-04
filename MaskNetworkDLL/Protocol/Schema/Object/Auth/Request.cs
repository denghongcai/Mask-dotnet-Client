// automatically generated, do not modify

namespace MaskGame.Protocol.Schema.Object.Auth
{

using System;
using FlatBuffers;

public sealed class Request : Table {
  public static Request GetRootAsRequest(ByteBuffer _bb) { return GetRootAsRequest(_bb, new Request()); }
  public static Request GetRootAsRequest(ByteBuffer _bb, Request obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Request __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string Nickname { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetNicknameBytes() { return __vector_as_arraysegment(4); }

  public static Offset<Request> CreateRequest(FlatBufferBuilder builder,
      StringOffset nicknameOffset = default(StringOffset)) {
    builder.StartObject(1);
    Request.AddNickname(builder, nicknameOffset);
    return Request.EndRequest(builder);
  }

  public static void StartRequest(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddNickname(FlatBufferBuilder builder, StringOffset nicknameOffset) { builder.AddOffset(0, nicknameOffset.Value, 0); }
  public static Offset<Request> EndRequest(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Request>(o);
  }
  public static void FinishRequestBuffer(FlatBufferBuilder builder, Offset<Request> offset) { builder.Finish(offset.Value); }
};


}
