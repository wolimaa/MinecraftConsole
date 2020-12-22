using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace MinecraftConsole
{
    public class MineStat
    {
        private const ushort DataSize = 512;  // this will hopefully suffice since the MotD should be <=59 characters
        private const ushort NumFields = 6;   // number of values expected from server
        private const int DefaultTimeout = 5; // default TCP timeout in seconds

        public string Address { get; set; }
        public ushort Port { get; set; }
        public int Timeout { get; set; }
        public string Motd { get; set; }
        public string Version { get; set; }
        public string CurrentPlayers { get; set; }
        public string MaximumPlayers { get; set; }
        public bool ServerUp { get; set; }
        public long Latency { get; set; }

        public MineStat(string address, ushort port, int timeout = DefaultTimeout)
        {
            var rawServerData = new byte[DataSize];

            Address = address;
            Port = port;
            Timeout = timeout * 1000;   // milliseconds

            try
            {
                var stopWatch = new Stopwatch();
                var tcpClient = new TcpClient { ReceiveTimeout = Timeout };
                stopWatch.Start();
                tcpClient.Connect(address, port);
                stopWatch.Stop();
                Latency = stopWatch.ElapsedMilliseconds;
                var stream = tcpClient.GetStream();
                var payload = new byte[] { 0xFE, 0x01 };
                stream.Write(payload, 0, payload.Length);
                stream.Read(rawServerData, 0, DataSize);
                tcpClient.Close();
            }
            catch (Exception)
            {
                ServerUp = false;
                return;
            }

            if (rawServerData.Length == 0)
            {
                ServerUp = false;
            }
            else
            {
                var serverData1 = Encoding.Unicode.GetString(rawServerData);
                var serverData = Encoding.Unicode.GetString(rawServerData).Split("\u0000\u0000\u0000".ToCharArray());
                if (serverData.Length >= NumFields)
                {
                    ServerUp = true;
                    Version = serverData[2];
                    Motd = serverData[3];
                    CurrentPlayers = serverData[4];
                    MaximumPlayers = serverData[5];
                }
                else
                {
                    ServerUp = false;
                }
            }
        }
    }
}