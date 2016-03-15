// automatically generated, do not modify

namespace MaskGame.Protocol.Schema.Object
{

using System;
using FlatBuffers;

public sealed class Error : Table {
  public static Error GetRootAsError(ByteBuffer _bb) { return GetRootAsError(_bb, new Error()); }
  public static Error GetRootAsError(ByteBuffer _bb, Error obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Error __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int Code { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public string Message { get { int o = __offset(6); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetMessageBytes() { return __vector_as_arraysegment(6); }

  public static Offset<Error> CreateError(FlatBufferBuilder builder,
      int code = 0,
      StringOffset messageOffset = default(StringOffset)) {
    builder.StartObject(2);
    Error.AddMessage(builder, messageOffset);
    Error.AddCode(builder, code);
    return Error.EndError(builder);
  }

  public static void StartError(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddCode(FlatBufferBuilder builder, int code) { builder.AddInt(0, code, 0); }
  public static void AddMessage(FlatBufferBuilder builder, StringOffset messageOffset) { builder.AddOffset(1, messageOffset.Value, 0); }
  public static Offset<Error> EndError(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Error>(o);
  }
  public static void FinishErrorBuffer(FlatBufferBuilder builder, Offset<Error> offset) { builder.Finish(offset.Value); }
};


}
