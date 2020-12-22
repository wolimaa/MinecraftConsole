using MinecraftConsole.Domain;
using MinecraftConsole.Domain.Exceptions;
using MinecraftConsole.Domain.Gateways;
using MinecraftConsole.Infrastructure.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MinecraftConsole.Infrastructure.Gateways
{
    public class MinecraftServerGateway : IMinecraftServerGateway
    {
        private static NetworkStream stream;
        private static List<byte> buffer;
        private static int offset;
        private const int PROTOCOL_VERSION = 47;
        private const int NEXT_STATE = 1;

        public PingPayload GetStream(string server, short port = 25565)
        {
            var client = new TcpClient();
            var task = client.ConnectAsync(server, 25565);

            while (!task.IsCompleted)
            {
                Thread.Sleep(250);
            }

            if (!client.Connected)
                throw new ServerExceptions($"Não foi possivel conectar no servidor: {server}");

            buffer = new List<byte>();
            stream = client.GetStream();

            return SendHandshakePacket(PROTOCOL_VERSION, server, port, NEXT_STATE);
        }

        private PingPayload SendHandshakePacket(int protocolVersion, string server, short port, int nextState)
        {
            /* Send a "Handshake" packet
             * http://wiki.vg/Server_List_Ping#Ping_Process */

            WriteVarInt(protocolVersion);
            WriteString(server);
            WriteShort(port);
            WriteVarInt(nextState);
            Flush(0);

            /* Send a "Status Request" packet
            * http://wiki.vg/Server_List_Ping#Ping_Process */
            Flush(0);

            /*
            * If you are using a modded server then use a larger buffer to account,
            * see link for explanation and a motd to HTML snippet
            * https://gist.github.com/csh/2480d14fbbb33b4bbae3#gistcomment-2672658
            */
            var buffer = new byte[Int16.MaxValue];
            // var buffer = new byte[4096];
            stream.Read(buffer, 0, buffer.Length);

            try
            {
                var length = NumericUtils.ReadVarInt(buffer);
                var packet = NumericUtils.ReadVarInt(buffer);
                var jsonLength = NumericUtils.ReadVarInt(buffer);
                var json = StringUtils.ReadString(buffer, jsonLength);
                var ping = JsonConvert.DeserializeObject<PingPayload>(json);
                return ping;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void WriteVarInt(int value)
        {
            while ((value & 128) != 0)
            {
                buffer.Add((byte)(value & 127 | 128));
                value = (int)((uint)value) >> 7;
            }
            buffer.Add((byte)value);
        }

        private static void WriteShort(short value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        private static void WriteString(string data)
        {
            var bufferData = Encoding.UTF8.GetBytes(data);
            WriteVarInt(bufferData.Length);
            buffer.AddRange(bufferData);
        }

        private static void Write(byte b)
        {
            stream.WriteByte(b);
        }

        private static void Flush(int id = -1)
        {
            var bufferData = buffer.ToArray();
            buffer.Clear();

            var add = 0;
            var packetData = new[] { (byte)0x00 };
            if (id >= 0)
            {
                WriteVarInt(id);
                packetData = buffer.ToArray();
                add = packetData.Length;
                buffer.Clear();
            }

            WriteVarInt(bufferData.Length + add);
            var bufferLength = buffer.ToArray();
            buffer.Clear();

            stream.Write(bufferLength, 0, bufferLength.Length);
            stream.Write(packetData, 0, packetData.Length);
            stream.Write(bufferData, 0, bufferData.Length);
        }
    }
}