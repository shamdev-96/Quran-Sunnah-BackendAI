# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Set the environment variable for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8080

# Expose the port that the application will listen on
EXPOSE 80

# Command to run the application
ENTRYPOINT ["dotnet", "Quran-Sunnah-BackendAI.dll"]
