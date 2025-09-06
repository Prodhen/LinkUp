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
# RUN mkdir -p /app/data

# Set environment variables
ARG CONFIG=Production
ENV ASPNETCORE_ENVIRONMENT=$CONFIG
ENV ASPNETCORE_URLS=http://+:80

# Expose port 80
EXPOSE 80

# Set the entry point
ENTRYPOINT ["dotnet", "API.dll"]

# To create a Docker network, use the following command:
# docker network create linkup-network

# To create a named volume for PostgreSQL data persistence, use the following command:
#docker volume create db-data-named

# To run PostgreSQL container, use the following command:
#docker run -d  --name db --network linkup-network -p 5432:5432 -e POSTGRES_USER=postgres -e 
#POSTGRES_PASSWORD=12345 -e POSTGRES_DB=TestDB -v db-data-named:/var/lib/postgresql/data postgres:15

# To build the Docker image, run:
#  docker build -t my-api-image .        
# To run the Docker container, run:
#docker run -d -p 5000:80 --network linkup-network --name api my-api-image:latest