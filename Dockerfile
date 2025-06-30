# -------- Build stage --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and all project files
COPY *.sln ./
COPY API/API.csproj ./API/
COPY DTO/DTO.csproj ./DTO/
COPY Repositories/Repositories.csproj ./Repositories/
COPY Services/Services.csproj ./Services/

# Restore dependencies
RUN dotnet restore

# Copy the full source
COPY . .

# Build and publish the API project
WORKDIR /src/API
RUN dotnet publish -c Release -o /app/publish

# -------- Runtime stage --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port
EXPOSE 80

# Set environment variable for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:80

# Start the app
ENTRYPOINT ["dotnet", "API.dll"]