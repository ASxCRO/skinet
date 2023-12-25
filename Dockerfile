# Use the official .NET 8 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the project files to the container
COPY . ./

# Stage 1: Build the Angular app
FROM node:18.7.1 as ng-builder
WORKDIR /app/client

# Install dependencies and build the Angular app
RUN npm install
RUN npm run build

WORKDIR /app
# Build the application
RUN dotnet publish -c Release -o publish skinet.sln

# Use the official .NET 8 Runtime image as the base image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the published output from the build stage to the runtime stage
COPY --from=build /app/publish ./

# Expose the port on which the application will run
EXPOSE 8080

# Define the entry point for the application
ENTRYPOINT ["dotnet", "API.dll"]