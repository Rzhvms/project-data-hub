FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["Apps/IdentityApi/Api/Api.csproj", "Apps/IdentityApi/Api/"]
COPY ["Apps/IdentityApi/Application/Application.csproj", "Apps/IdentityApi/Application/"]
COPY ["Apps/IdentityApi/Domain/Domain.csproj", "Apps/IdentityApi/Domain/"]
COPY ["Apps/IdentityApi/Infrastructure/Infrastructure.csproj", "Apps/IdentityApi/Infrastructure/"]

COPY ["Libs/CoreLib/CoreLib/CoreLib.csproj", "Libs/CoreLib/CoreLib/"]
COPY ["Libs/IdentityLib/IdentityLib/IdentityLib.csproj", "Libs/IdentityLib/IdentityLib/"]

RUN dotnet restore "Apps/IdentityApi/Api/Api.csproj"
COPY . .
WORKDIR "/src/Apps/IdentityApi/Api"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
