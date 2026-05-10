FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["Apps/ProviderApi/Api/Api.csproj", "Apps/ProviderApi/Api/"]
COPY ["Apps/ProviderApi/Application/Application.csproj", "Apps/ProviderApi/Application/"]
COPY ["Apps/ProviderApi/Domain/Domain.csproj", "Apps/ProviderApi/Domain/"]
COPY ["Apps/ProviderApi/Infrastructure/Infrastructure.csproj", "Apps/ProviderApi/Infrastructure/"]

COPY ["Libs/CoreLib/CoreLib/CoreLib.csproj", "Libs/CoreLib/CoreLib/"]

RUN dotnet restore "Apps/ProviderApi/Api/Api.csproj"
COPY . .
WORKDIR "/src/Apps/ProviderApi/Api"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
