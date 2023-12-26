# Stage 1: Build Angular App
FROM node:18.17.1 as angular-builder
WORKDIR /app
COPY client/package.json client/package-lock.json ./
RUN npm install
COPY client .
RUN npm run build --prod

# Stage 2: Build .NET API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy necessary files for .NET build
COPY . ./

# Publish the .NET API
RUN dotnet publish -c Release -o out skinet.sln

# Stage 3: Create the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published .NET API
COPY --from=build /app/out .

# Copy the Angular production files to the wwwroot folder
COPY --from=angular-builder /app/dist ./wwwroot

# Expose port
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "API.dll"]
