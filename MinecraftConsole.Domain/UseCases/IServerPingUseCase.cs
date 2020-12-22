using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftConsole.Domain.UseCases
{
    public interface IServerPingUseCase
    {
       Task<PingPayload> Execute();
    }
}
