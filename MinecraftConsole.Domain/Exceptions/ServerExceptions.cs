using System;

namespace MinecraftConsole.Domain.Exceptions
{
    public class ServerExceptions : Exception
    {
        public ServerExceptions(string message)
       : base(message) { }
    }
}