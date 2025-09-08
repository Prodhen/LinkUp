# Stage 1: Build the Angular application
FROM node:20-alpine AS build

# Set working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies (use npm install instead of npm ci for Docker)
RUN npm install

# Copy source code
COPY . .

# Build the application for production
RUN npm run build -- --configuration production

# Stage 2: Serve with nginx
FROM nginx:alpine AS runtime

# Remove default nginx website
RUN rm -rf /usr/share/nginx/html/*

# Copy the built application from build stage
# Update the path to match Angular's default output folder
COPY --from=build /app/dist/client /usr/share/nginx/html

# Copy custom nginx configuration
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 80
EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"]
