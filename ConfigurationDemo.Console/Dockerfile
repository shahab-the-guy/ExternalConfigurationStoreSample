FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ConfigurationDemo.Console/ConfigurationDemo.Console.csproj", "ConfigurationDemo.Console/"]
RUN dotnet restore "ConfigurationDemo.Console/ConfigurationDemo.Console.csproj"
COPY . .
WORKDIR "/src/ConfigurationDemo.Console"
RUN dotnet build "ConfigurationDemo.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConfigurationDemo.Console.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConfigurationDemo.Console.dll"]
