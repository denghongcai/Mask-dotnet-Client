// automatically generated, do not modify

namespace MaskGame.Protocol.Schema.Object
{

using System;
using FlatBuffers;

public sealed class Player : Table {
  public static Player GetRootAsPlayer(ByteBuffer _bb) { return GetRootAsPlayer(_bb, new Player()); }
  public static Player GetRootAsPlayer(ByteBuffer _bb, Player obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Player __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public Vec3 Pos { get { return GetPos(new Vec3()); } }
  public Vec3 GetPos(Vec3 obj) { int o = __offset(4); return o != 0 ? obj.__init(o + bb_pos, bb) : null; }
  public short Mana { get { int o = __offset(6); return o != 0 ? bb.GetShort(o + bb_pos) : (short)150; } }
  public short Hp { get { int o = __offset(8); return o != 0 ? bb.GetShort(o + bb_pos) : (short)100; } }
  public string Name { get { int o = __offset(10); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetNameBytes() { return __vector_as_arraysegment(10); }
  public bool Friendly { get { int o = __offset(12); return o != 0 ? 0!=bb.Get(o + bb_pos) : (bool)false; } }
  public Color Color { get { int o = __offset(14); return o != 0 ? (Color)bb.GetSbyte(o + bb_pos) : Color.Blue; } }

  public static void StartPlayer(FlatBufferBuilder builder) { builder.StartObject(6); }
  public static void AddPos(FlatBufferBuilder builder, Offset<Vec3> posOffset) { builder.AddStruct(0, posOffset.Value, 0); }
  public static void AddMana(FlatBufferBuilder builder, short mana) { builder.AddShort(1, mana, 150); }
  public static void AddHp(FlatBufferBuilder builder, short hp) { builder.AddShort(2, hp, 100); }
  public static void AddName(FlatBufferBuilder builder, StringOffset nameOffset) { builder.AddOffset(3, nameOffset.Value, 0); }
  public static void AddFriendly(FlatBufferBuilder builder, bool friendly) { builder.AddBool(4, friendly, false); }
  public static void AddColor(FlatBufferBuilder builder, Color color) { builder.AddSbyte(5, (sbyte)color, 2); }
  public static Offset<Player> EndPlayer(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Player>(o);
  }
  public static void FinishPlayerBuffer(FlatBufferBuilder builder, Offset<Player> offset) { builder.Finish(offset.Value); }
};


}
