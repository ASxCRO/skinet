

# Stage 2: Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the project files to the container
COPY . ./

# Build the .NET application
RUN dotnet publish -c Release -o publish skinet.sln

# Stage 1: Build the Angular app
FROM node:18.17.1 as ng-builder
WORKDIR /app/client

# Install dependencies and build the Angular app
RUN npm install
RUN npm run build

COPY --from=ng-builder /app/API/wwwroot /app/publish/wwwroot

# Stage 3: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory in the container
WORKDIR /app

# Copy the published output from the build stage to the runtime stage
COPY --from=build /app/publish ./

# Expose the port on which the application will run
EXPOSE 8080

# Define the entry point for the application
ENTRYPOINT ["dotnet", "API.dll"]