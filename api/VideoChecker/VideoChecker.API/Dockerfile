# Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5056
ENV ASPNETCORE_URLS=http://+:5056

# SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia só os csproj para cache eficiente
COPY api/VideoChecker/VideoChecker.API/VideoChecker.API.csproj VideoChecker.API/
COPY api/VideoChecker/VideoChecker.Data/VideoChecker.Data.csproj VideoChecker.Data/
COPY api/VideoChecker/VideoChecker.Domain/VideoChecker.Domain.csproj VideoChecker.Domain/
COPY SharedEntities/SharedEntities.csproj SharedEntities/

# Restaura a partir da API (segue os ProjectReference)
RUN dotnet restore "VideoChecker.API/VideoChecker.API.csproj"

# Copia o restante do código
COPY api/VideoChecker/ ./
COPY SharedEntities/ ./SharedEntities/

ARG BUILD_CONFIGURATION=Release
RUN dotnet build "VideoChecker.API/VideoChecker.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "VideoChecker.API/VideoChecker.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VideoChecker.API.dll"]
