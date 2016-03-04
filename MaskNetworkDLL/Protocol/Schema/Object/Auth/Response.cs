// automatically generated, do not modify

namespace MaskGame.Protocol.Schema.Object.Auth
{

using System;
using FlatBuffers;

public sealed class Response : Table {
  public static Response GetRootAsResponse(ByteBuffer _bb) { return GetRootAsResponse(_bb, new Response()); }
  public static Response GetRootAsResponse(ByteBuffer _bb, Response obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Response __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int Code { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public string Error { get { int o = __offset(6); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetErrorBytes() { return __vector_as_arraysegment(6); }

  public static Offset<Response> CreateResponse(FlatBufferBuilder builder,
      int code = 0,
      StringOffset errorOffset = default(StringOffset)) {
    builder.StartObject(2);
    Response.AddError(builder, errorOffset);
    Response.AddCode(builder, code);
    return Response.EndResponse(builder);
  }

  public static void StartResponse(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddCode(FlatBufferBuilder builder, int code) { builder.AddInt(0, code, 0); }
  public static void AddError(FlatBufferBuilder builder, StringOffset errorOffset) { builder.AddOffset(1, errorOffset.Value, 0); }
  public static Offset<Response> EndResponse(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Response>(o);
  }
  public static void FinishResponseBuffer(FlatBufferBuilder builder, Offset<Response> offset) { builder.Finish(offset.Value); }
};


}
