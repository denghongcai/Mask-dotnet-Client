// automatically generated, do not modify

namespace MaskGame.Protocol.Schema.Object
{

using System;
using FlatBuffers;

public sealed class VoiceSegment : Table {
  public static VoiceSegment GetRootAsVoiceSegment(ByteBuffer _bb) { return GetRootAsVoiceSegment(_bb, new VoiceSegment()); }
  public static VoiceSegment GetRootAsVoiceSegment(ByteBuffer _bb, VoiceSegment obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public VoiceSegment __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string Playerid { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetPlayeridBytes() { return __vector_as_arraysegment(4); }
  public byte GetData(int j) { int o = __offset(6); return o != 0 ? bb.Get(__vector(o) + j * 1) : (byte)0; }
  public int DataLength { get { int o = __offset(6); return o != 0 ? __vector_len(o) : 0; } }
  public ArraySegment<byte>? GetDataBytes() { return __vector_as_arraysegment(6); }

  public static Offset<VoiceSegment> CreateVoiceSegment(FlatBufferBuilder builder,
      StringOffset playeridOffset = default(StringOffset),
      VectorOffset dataOffset = default(VectorOffset)) {
    builder.StartObject(2);
    VoiceSegment.AddData(builder, dataOffset);
    VoiceSegment.AddPlayerid(builder, playeridOffset);
    return VoiceSegment.EndVoiceSegment(builder);
  }

  public static void StartVoiceSegment(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddPlayerid(FlatBufferBuilder builder, StringOffset playeridOffset) { builder.AddOffset(0, playeridOffset.Value, 0); }
  public static void AddData(FlatBufferBuilder builder, VectorOffset dataOffset) { builder.AddOffset(1, dataOffset.Value, 0); }
  public static VectorOffset CreateDataVector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static void StartDataVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static Offset<VoiceSegment> EndVoiceSegment(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<VoiceSegment>(o);
  }
  public static void FinishVoiceSegmentBuffer(FlatBufferBuilder builder, Offset<VoiceSegment> offset) { builder.Finish(offset.Value); }
};


}
