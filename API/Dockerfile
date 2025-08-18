# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj file and restore dependencies
COPY API.csproj ./
RUN dotnet restore API.csproj

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build API.csproj -c Release -o /app/build

# Publish the application
RUN dotnet publish API.csproj -c Release -o /app/publish

# Use the runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Create directory for SQLite database (Linkup.db)
RUN mkdir -p /app/data

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80

# Expose port 80
EXPOSE 80

# Set the entry point
ENTRYPOINT ["dotnet", "API.dll"]