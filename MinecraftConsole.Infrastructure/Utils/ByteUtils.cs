using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftConsole.Infrastructure.Utils
{
   public class ByteUtils
    {
        private static List<byte> buffer;
        private static int offset;

        public static byte ReadByte(byte[] buffer)
        {
            var b = buffer[offset];
            offset += 1;
            return b;
        }

        public static byte[] Read(byte[] buffer, int length)
        {
            var data = new byte[length];
            Array.Copy(buffer, offset, data, 0, length);
            offset += length;
            return data;
        }
    }
}
