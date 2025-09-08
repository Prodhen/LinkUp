# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY API.csproj ./
RUN dotnet restore API.csproj

# Copy everything else
COPY . .

# Build
RUN dotnet build API.csproj -c Release -o /app/build

# Publish
RUN dotnet publish API.csproj -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "API.dll"]