FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5056
ENV ASPNETCORE_URLS=http://+:5056

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY api/VideoChecker/VideoChecker.API/VideoChecker.API.csproj api/VideoChecker/VideoChecker.API/
COPY api/VideoChecker/VideoChecker.Data/VideoChecker.Data.csproj api/VideoChecker/VideoChecker.Data/
COPY api/VideoChecker/VideoChecker.Domain/VideoChecker.Domain.csproj api/VideoChecker/VideoChecker.Domain/
COPY api/VideoChecker/VideoChecker.Infra/VideoChecker.Infra.csproj api/VideoChecker/VideoChecker.Infra/
COPY SharedEntities/SharedEntities.csproj SharedEntities/

RUN dotnet restore "api/VideoChecker/VideoChecker.API/VideoChecker.API.csproj"

COPY api/VideoChecker/ api/VideoChecker/
COPY SharedEntities/ SharedEntities/

ARG BUILD_CONFIGURATION=Release
RUN dotnet build "api/VideoChecker/VideoChecker.API/VideoChecker.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "api/VideoChecker/VideoChecker.API/VideoChecker.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VideoChecker.API.dll"]
