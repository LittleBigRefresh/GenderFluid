using System.Buffers.Binary;

namespace GenderFluid;

public class BEBinaryWriter : BinaryWriter
{
    public BEBinaryWriter(Stream input) : base(input)
    {}

    public override void Write(ulong value)
    {
        Span<byte> buf = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64BigEndian(buf, value);
        base.Write(buf);
    }
    
    public override void Write(long value)
    {
        Span<byte> buf = stackalloc byte[sizeof(long)];
        BinaryPrimitives.WriteInt64BigEndian(buf, value);
        base.Write(buf);
    }
    
    public override void Write(uint value)
    {
        Span<byte> buf = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32BigEndian(buf, value);
        base.Write(buf);
    }
    
    public override void Write(int value)
    {
        Span<byte> buf = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32BigEndian(buf, value);
        base.Write(buf);
    }
    
    public override void Write(short value)
    {
        Span<byte> buf = stackalloc byte[sizeof(short)];
        BinaryPrimitives.WriteInt16BigEndian(buf, value);
        base.Write(buf);
    }
    
    public override void Write(ushort value)
    {
        Span<byte> buf = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16BigEndian(buf, value);
        base.Write(buf);
    }
}