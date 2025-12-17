FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy Server project
COPY ChatApp.Server/ ./ChatApp.Server/

# Debug: List files to see what was copied
WORKDIR /src/ChatApp.Server
RUN ls -la
RUN echo "===== Looking for csproj files ====="
RUN find . -name "*.csproj" || echo "No csproj files found"

# Restore and build
RUN dotnet restore || echo "Restore failed - checking directory contents again" && ls -la
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Railway provides PORT environment variable
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-5000}

ENTRYPOINT ["dotnet", "ChatApp.Server.dll"]
