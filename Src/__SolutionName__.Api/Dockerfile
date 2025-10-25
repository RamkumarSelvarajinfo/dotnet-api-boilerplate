# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files for all projects first (for better caching)
COPY ["Src/__SolutionName__.Api/__SolutionName__.Api.csproj", "Src/__SolutionName__.Api/"]
COPY ["Src/__SolutionName__.Application/__SolutionName__.Application.csproj", "Src/__SolutionName__.Application/"]
COPY ["Src/__SolutionName__.Domain/__SolutionName__.Domain.csproj", "Src/__SolutionName__.Domain/"]
COPY ["Src/__SolutionName__.Infrastructure/__SolutionName__.Infrastructure.csproj", "Src/__SolutionName__.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Src/__SolutionName__.Api/__SolutionName__.Api.csproj"

# Copy everything else
COPY . .

# Build and publish the API
RUN dotnet publish "Src/__SolutionName__.Api/__SolutionName__.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "__SolutionName__.Api.dll"]
