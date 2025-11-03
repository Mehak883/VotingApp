# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj(s) and restore first for layer cache
COPY *.sln ./
COPY MyApi/*.csproj MyApi/
COPY tests/*.csproj tests/

# restore (nuget cache will be a good candidate for Harness cache)
RUN dotnet restore

# copy everything and build
COPY . .
WORKDIR /src/MyApi
RUN dotnet publish -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "MyApi.dll"]
