using System;

namespace VisiMatrix.FileFormat
{
    public class TrackHeader
    {
        public Byte Version;
        public Byte[] Flags; //3 bytes
        public UInt32 CreationTime;
        public UInt32 ModificatonTime;
        public UInt32 TrackId;
        public UInt32 Reserved1;
        public UInt32 Duration;
        public UInt64 Reserved2;
        public UInt16 Layer;
        public UInt16 AlternateGroup;
        public Single Volume;  //Actually 8.8 floating point!
        public UInt16 Reserved3;
        public AppleMatrix Matrix;
        public Single Width;  //Actually 16.16 floating point!
        public Single Heigth; //Actually 16.16 floating point!

        public TrackHeader(BigEndianBinaryReader stream)
        {
            Version = stream.ReadByte();
            Flags = stream.ReadBytes(3);
            CreationTime = stream.ReadUInt32();
            ModificatonTime = stream.ReadUInt32();
            TrackId = stream.ReadUInt32();
            Reserved1 = stream.ReadUInt32();
            Duration = stream.ReadUInt32();
            Reserved2 = stream.ReadUInt64();
            Layer = stream.ReadUInt16();
            AlternateGroup = stream.ReadUInt16();
            Volume = Fixed.Float(stream.ReadUInt16(), 8);
            Reserved3 = stream.ReadUInt16();
            Matrix = new AppleMatrix(stream);
            Width = Fixed.Float(stream.ReadUInt32(), 16);
            Heigth = Fixed.Float(stream.ReadUInt32(), 16);
        }
    }
}