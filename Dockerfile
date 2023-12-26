# Stage 1: Build Angular app
FROM node:18.17.1 as angular-builder

WORKDIR /app

COPY client/package*.json ./
RUN npm install

COPY client/ .
RUN npm run build --prod

# Stage 2: Build .NET API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet-builder

WORKDIR /app

COPY API/API.csproj .
RUN dotnet restore

COPY API/ .
COPY --from=angular-builder /app/dist/wwwroot ./wwwroot

RUN dotnet publish -c Release -o out

# Stage 3: Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=dotnet-builder /app/out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "API.dll"]
