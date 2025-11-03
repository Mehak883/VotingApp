# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy project files
COPY VotingApp/VotingApp.csproj VotingApp/
RUN dotnet restore VotingApp/VotingApp.csproj

# copy the rest of the source
COPY . .
WORKDIR /src/VotingApp
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "VotingApp.dll"]
