using MinecraftConsole.Domain;
using MinecraftConsole.Domain.UseCases;
using System;
using System.Threading.Tasks;

namespace MinecraftConsole.Application
{
    public class ServerPingUseCase : IServerPingUseCase
    {
        public Task<PingPayload> Execute()
        {
            throw new NotImplementedException();
        }
    }
}