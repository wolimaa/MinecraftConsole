using System.Text;

namespace MinecraftConsole.Infrastructure.Utils
{
    public class StringUtils
    {
        public static string ReadString(byte[] buffer, int length)
        {
            var data = ByteUtils.Read(buffer, length);
            return Encoding.UTF8.GetString(data);
        }

    }
}