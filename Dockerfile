# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 8080

# Copy the solution file
COPY *.sln ./

# Copy all project files
COPY WebAPI/WebAPI.csproj ./WebAPI/
COPY Repositories/Repositories.csproj ./Repositories/
COPY Services/Services.csproj ./Services/
COPY Domain/Domain.csproj ./Domain/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build the project
RUN dotnet build -c Release

# Publish stage
FROM build AS publish
WORKDIR /app/WebAPI
RUN dotnet publish -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]