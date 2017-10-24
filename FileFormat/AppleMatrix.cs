using System;
using System.IO;
using System.Windows.Media;

namespace VisiMatrix.FileFormat
{
    public class AppleMatrix
    {
        public Single M11, M12, M13, M21, M22, M23, M31, M32, M33;
        public readonly long Location;

        public AppleMatrix(BigEndianBinaryReader stream)
        {
            Location = stream.BaseStream.Position;
            M11 = Fixed.Float(stream.ReadUInt32(), 16);
            M12 = Fixed.Float(stream.ReadUInt32(), 16);
            M13 = Fixed.Float(stream.ReadUInt32(), 30);
            M21 = Fixed.Float(stream.ReadUInt32(), 16);
            M22 = Fixed.Float(stream.ReadUInt32(), 16);
            M23 = Fixed.Float(stream.ReadUInt32(), 30);
            M31 = Fixed.Float(stream.ReadUInt32(), 16);
            M32 = Fixed.Float(stream.ReadUInt32(), 16);
            M33 = Fixed.Float(stream.ReadUInt32(), 30);
        }

        public AppleMatrix(Matrix matrix)
        {
            Location = -1;
            M11 = (Single) matrix.M11;
            M12 = (Single) matrix.M12;
            M13 = 0;
            M21 = (Single) matrix.M21;
            M22 = (Single) matrix.M22;
            M23 = 0;
            M31 = (Single) matrix.OffsetX;
            M32 = (Single) matrix.OffsetY;
            M33 = 1;
        }

        public Matrix ToMatrix()
        {
            return new Matrix(M11, M12, M21, M22, M31, M32);
        }

        public void Write(BinaryWriter stream)
        {
            WriteUint32LE(stream, Fixed.Int(M11, 16));
            WriteUint32LE(stream, Fixed.Int(M12, 16));
            WriteUint32LE(stream, Fixed.Int(M13, 30));
            WriteUint32LE(stream, Fixed.Int(M21, 16));
            WriteUint32LE(stream, Fixed.Int(M22, 16));
            WriteUint32LE(stream, Fixed.Int(M23, 30));
            WriteUint32LE(stream, Fixed.Int(M31, 16));
            WriteUint32LE(stream, Fixed.Int(M32, 16));
            WriteUint32LE(stream, Fixed.Int(M33, 30));
        }

        private static void WriteUint32LE(BinaryWriter stream, UInt32 data)
        {
            for (int i = 24; i >= 0; i -= 8) stream.Write((byte) ((data & (0xff << i)) >> i));
        }
    }
}