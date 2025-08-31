# Stage 1: Build Angular app
FROM node:20-alpine AS build

# Set working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm ci

# Copy the rest of the source code
COPY . .

# Build Angular app
ARG CONFIG=production
RUN npm run build -- --configuration $CONFIG

# Stage 2: Run with Node.js (no nginx)
FROM node:20-alpine AS runtime

WORKDIR /app

# Install a simple static server for Angular
RUN npm install -g http-server

# Copy built Angular app from build stage
COPY --from=build /app/dist/client/browser /app

# Expose port
EXPOSE 4200

# Start http-server
CMD ["http-server", "-p", "80"]
#CMD ["http-server", "-p", "4200"] there is no problem.
