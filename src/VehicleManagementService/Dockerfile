# Use an official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /src

# Copy the .csproj files and restore dependencies
COPY ["src/VehicleManagementService/VehicleManagementService.csproj", "VehicleManagementService/"]
RUN dotnet restore "VehicleManagementService/VehicleManagementService.csproj"

# Copy the rest of the source code
COPY src/ .

# Build the VehicleManagementService
WORKDIR "/src/VehicleManagementService"
RUN dotnet publish "VehicleManagementService.csproj" -c Release -o /app/publish

# Use an official .NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "VehicleManagementService.dll"]
