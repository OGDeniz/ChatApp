FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Restore and build
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Railway provides PORT environment variable
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-5000}

ENTRYPOINT ["dotnet", "ChatApp.Server.dll"]
