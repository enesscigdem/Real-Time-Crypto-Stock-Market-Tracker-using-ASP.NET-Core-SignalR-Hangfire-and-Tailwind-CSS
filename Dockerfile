# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Real-Time-Crypto-Stock-Market-Tracker-using-ASP.NET-Core-SignalR-Hangfire-and-Tailwind-CSS.csproj", "./"]
RUN dotnet restore "Real-Time-Crypto-Stock-Market-Tracker-using-ASP.NET-Core-SignalR-Hangfire-and-Tailwind-CSS.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Real-Time-Crypto-Stock-Market-Tracker-using-ASP.NET-Core-SignalR-Hangfire-and-Tailwind-CSS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Real-Time-Crypto-Stock-Market-Tracker-using-ASP.NET-Core-SignalR-Hangfire-and-Tailwind-CSS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Real-Time-Crypto-Stock-Market-Tracker-using-ASP.NET-Core-SignalR-Hangfire-and-Tailwind-CSS.dll"]
