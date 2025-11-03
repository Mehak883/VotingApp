# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY VotingApp.sln ./
COPY VotingApp.API/VotingApp.API.csproj VotingApp.API/
RUN dotnet restore VotingApp.API/VotingApp.API.csproj

# Copy all source and build
COPY . .
WORKDIR /src/VotingApp.API
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "VotingApp.API.dll"]
