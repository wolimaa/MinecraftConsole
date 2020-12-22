using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MinecraftConsole.Infrastructure.Utils
{
    public class NumericUtils
    {
        public static int ReadVarInt(byte[] buffer)
        {
            var value = 0;
            var size = 0;
            int b;
            while (((b = ByteUtils.ReadByte(buffer)) & 0x80) == 0x80)
            {
                value |= (b & 0x7F) << (size++ * 7);
                if (size > 5)
                {
                    throw new IOException("This VarInt is an imposter!");
                }
            }
            return value | ((b & 0x7F) << (size * 7));
        }

    }
}
