# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 8080

# Copy the solution file and project files
COPY *.sln ./
COPY Domain/Domain.csproj ./Domain/
COPY Repositories/Repositories.csproj ./Repositories/
COPY Services/Services.csproj ./Services/
COPY WebAPI/WebAPI.csproj ./WebAPI/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code and build the project
COPY . .
RUN dotnet build -c Release

# Publish the application
FROM build AS publish
WORKDIR /app/WebAPI
RUN dotnet publish -c Release -o /app/publish

# Final stage: use the ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]
