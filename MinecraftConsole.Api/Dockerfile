#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MinecraftConsole.Api/MinecraftConsole.Api.csproj", "MinecraftConsole.Api/"]
RUN dotnet restore "MinecraftConsole.Api/MinecraftConsole.Api.csproj"
COPY . .
WORKDIR "/src/MinecraftConsole.Api"
RUN dotnet build "MinecraftConsole.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MinecraftConsole.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinecraftConsole.Api.dll"]