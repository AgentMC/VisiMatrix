using System;
using System.IO;

namespace VisiMatrix.FileFormat
{
    public class Atom
    {
        public UInt64 Size;
        public String Type;
        public Byte HeaderSize;

        public static Atom Read(BigEndianBinaryReader reader)
        {
            var atom = new Atom
            {
                Size = reader.ReadUInt32(),
                Type = new string(reader.ReadChars(4)),
                HeaderSize = 8
            };
            if (atom.Size == 1)
            {
                atom.Size = reader.ReadUInt64();
                atom.HeaderSize += 8;
            }
            return atom;
        }

        public Atom SkipContent(BigEndianBinaryReader reader)
        {
            reader.BaseStream.Seek((long) Size - HeaderSize, SeekOrigin.Current);
            return Read(reader);
        }

        public static Atom SeekToChild(BigEndianBinaryReader reader, string childName)
        {
            var atom = Read(reader);
            while (atom.Type != childName) atom = atom.SkipContent(reader);
            return atom;
        }
    }
}