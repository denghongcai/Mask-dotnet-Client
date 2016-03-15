// automatically generated, do not modify

namespace MaskGame.Protocol.Schema.Object
{

using System;
using FlatBuffers;

public sealed class Player : Table {
  public static Player GetRootAsPlayer(ByteBuffer _bb) { return GetRootAsPlayer(_bb, new Player()); }
  public static Player GetRootAsPlayer(ByteBuffer _bb, Player obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Player __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string Id { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetIdBytes() { return __vector_as_arraysegment(4); }
  public string Nickname { get { int o = __offset(6); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetNicknameBytes() { return __vector_as_arraysegment(6); }
  public Vec3 Pos { get { return GetPos(new Vec3()); } }
  public Vec3 GetPos(Vec3 obj) { int o = __offset(8); return o != 0 ? obj.__init(o + bb_pos, bb) : null; }
  public Vec3 Direction { get { return GetDirection(new Vec3()); } }
  public Vec3 GetDirection(Vec3 obj) { int o = __offset(10); return o != 0 ? obj.__init(o + bb_pos, bb) : null; }
  public bool Jump { get { int o = __offset(12); return o != 0 ? 0!=bb.Get(o + bb_pos) : (bool)false; } }
  public Face Face { get { int o = __offset(14); return o != 0 ? (Face)bb.GetSbyte(o + bb_pos) : Face.Regular; } }
  public bool Friendly { get { int o = __offset(16); return o != 0 ? 0!=bb.Get(o + bb_pos) : (bool)false; } }

  public static void StartPlayer(FlatBufferBuilder builder) { builder.StartObject(7); }
  public static void AddId(FlatBufferBuilder builder, StringOffset idOffset) { builder.AddOffset(0, idOffset.Value, 0); }
  public static void AddNickname(FlatBufferBuilder builder, StringOffset nicknameOffset) { builder.AddOffset(1, nicknameOffset.Value, 0); }
  public static void AddPos(FlatBufferBuilder builder, Offset<Vec3> posOffset) { builder.AddStruct(2, posOffset.Value, 0); }
  public static void AddDirection(FlatBufferBuilder builder, Offset<Vec3> directionOffset) { builder.AddStruct(3, directionOffset.Value, 0); }
  public static void AddJump(FlatBufferBuilder builder, bool jump) { builder.AddBool(4, jump, false); }
  public static void AddFace(FlatBufferBuilder builder, Face face) { builder.AddSbyte(5, (sbyte)face, 0); }
  public static void AddFriendly(FlatBufferBuilder builder, bool friendly) { builder.AddBool(6, friendly, false); }
  public static Offset<Player> EndPlayer(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Player>(o);
  }
  public static void FinishPlayerBuffer(FlatBufferBuilder builder, Offset<Player> offset) { builder.Finish(offset.Value); }
};


}
