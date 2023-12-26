# Stage 2: Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the project files to the container
COPY . ./

# Stage 2: Build the .NET application and the frontend
FROM node:18.17.1 AS frontend-build

# Set the working directory for the frontend
WORKDIR /app/client

# Copy the project files to the container
COPY ./client /app/client

# Install dependencies and build the frontend
RUN npm install
RUN npm run build

WORKDIR /app

# Build the .NET application
RUN dotnet publish -c Release -o publish skinet.sln

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