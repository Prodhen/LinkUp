# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj file and restore dependencies
COPY API/API.csproj ./API/
RUN dotnet restore ./API/API.csproj

# Copy the rest of the source code
COPY API/. ./API/

# Build the application
RUN dotnet build ./API/API.csproj -c Release -o /app/build

# Publish the application
RUN dotnet publish ./API/API.csproj -c Release -o /app/publish

# Use the runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Set environment variables
ARG CONFIG=Production
ENV ASPNETCORE_ENVIRONMENT=$CONFIG
ENV ASPNETCORE_URLS=http://+:80

# Expose port 80
EXPOSE 80

# Set the entry point
ENTRYPOINT ["dotnet", "API.dll"]
