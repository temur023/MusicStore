# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["MusicStore/MusicStore.csproj", "MusicStore/"]
RUN dotnet restore "MusicStore/MusicStore.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/MusicStore"
RUN dotnet publish "MusicStore.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Railway sets PORT env var, ASP.NET needs to listen on it
ENV ASPNETCORE_URLS=http://0.0.0.0:$PORT
ENTRYPOINT ["dotnet", "MusicStore.dll"]
