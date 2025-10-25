# Use the official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and restore as distinct layers
COPY ["__SolutionName__.Api/__SolutionName__.Api.csproj", "__SolutionName__.Api/"]
COPY ["__SolutionName__.Application/__SolutionName__.Application.csproj", "__SolutionName__.Application/"]
COPY ["__SolutionName__.Infrastructure/__SolutionName__.Infrastructure.csproj", "__SolutionName__.Infrastructure/"]
COPY . .
RUN dotnet restore "__SolutionName__.Api/__SolutionName__.Api.csproj"

# Build and publish the app
RUN dotnet publish "__SolutionName__.Api/__SolutionName__.Api.csproj" -c Release -o /app/publish

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
EXPOSE 443

# Set environment variables if needed
# ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "__SolutionName__.Api.dll"]