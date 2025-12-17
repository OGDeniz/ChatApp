FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ChatApp.Server.csproj .
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Railway provides PORT environment variable
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-5000}

ENTRYPOINT ["dotnet", "ChatApp.Server.dll"]
