FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Stock.Api/Stock.Api.csproj", "Stock.Api/"]
COPY ["Stock.Infrastructure/Stock.Infrastructure.csproj", "Stock.Infrastructure/"]
COPY ["Stock.Application/Stock.Application.csproj", "Stock.Application/"]
COPY ["Stock.Domain/Stock.Domain.csproj", "Stock.Domain/"]
RUN dotnet restore "./Stock.Api/Stock.Api.csproj"
COPY . .
WORKDIR "/src/Stock.Api"
RUN dotnet build "./Stock.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Stock.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stock.Api.dll"]