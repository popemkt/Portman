FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Portman.Api/Portman.Api.csproj", "src/Portman.Api/"]
RUN dotnet restore "src/Portman.Api/Portman.Api.csproj"
COPY . .
WORKDIR "/src/src/Portman.Api"
RUN dotnet build "Portman.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Portman.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Portman.Api.dll"]
