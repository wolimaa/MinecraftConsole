namespace MinecraftConsole.Domain.Gateways
{
    public interface IMinecraftServerGateway
    {
        PingPayload GetStream(string server, short port);
    }
}