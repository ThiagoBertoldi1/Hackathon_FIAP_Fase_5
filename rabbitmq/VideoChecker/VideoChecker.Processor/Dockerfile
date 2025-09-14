FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

RUN apt-get update \
 && apt-get install -y --no-install-recommends ffmpeg \
 && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY rabbitmq/VideoChecker/VideoChecker.Processor/VideoChecker.Processor.csproj rabbitmq/VideoChecker/VideoChecker.Processor/
COPY SharedEntities/SharedEntities.csproj SharedEntities/

RUN dotnet restore "rabbitmq/VideoChecker/VideoChecker.Processor/VideoChecker.Processor.csproj"

COPY rabbitmq/VideoChecker/ rabbitmq/VideoChecker/
COPY SharedEntities/ SharedEntities/

RUN dotnet build "rabbitmq/VideoChecker/VideoChecker.Processor/VideoChecker.Processor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "rabbitmq/VideoChecker/VideoChecker.Processor/VideoChecker.Processor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VideoChecker.Processor.dll"]
