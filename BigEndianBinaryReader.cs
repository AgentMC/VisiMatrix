using System.IO;
using System.Text;

namespace VisiMatrix
{
    public class BigEndianBinaryReader : BinaryReader
    {
        public BigEndianBinaryReader(Stream input) : base(input)
        {
        }

        public BigEndianBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
        {
        }

        public BigEndianBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
        }

        public override ushort ReadUInt16()
        {
            return (ushort) ((ReadByte() << 8) + ReadByte());
        }

        public override uint ReadUInt32()
        {
            return (uint) ((ReadByte() << 24) + (ReadByte() << 16) + (ReadByte() << 8) + ReadByte());
        }

        public override ulong ReadUInt64()
        {
            return ((ulong) ReadByte() << 56) +
                   ((ulong) ReadByte() << 48) +
                   ((ulong) ReadByte() << 40) +
                   ((ulong) ReadByte() << 32) +
                   ((ulong) ReadByte() << 24) +
                   ((ulong) ReadByte() << 16) +
                   ((ulong) ReadByte() << 8) +
                   ReadByte();
        }
    }
}