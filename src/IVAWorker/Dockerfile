# Use an official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /src

# Copy the .csproj files and restore dependencies
COPY ["src/IVAWorker/IVAWorker.csproj", "IVAWorker/"]
RUN dotnet restore "IVAWorker/IVAWorker.csproj"

# Copy the rest of the source code
COPY src/ .

# Build the IVAWorker
WORKDIR "/src/IVAWorker"
RUN dotnet publish "IVAWorker.csproj" -c Release -o /app/publish

# Use an official .NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

COPY src/IVAWorker/appsettings.json ./
COPY src/IVAWorker/appsettings.Development.json ./
COPY src/IVAWorker/appsettings.Docker.json ./

# Set the entry point for the container
ENTRYPOINT ["dotnet", "IVAWorker.dll"]
