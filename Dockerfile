
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["MusicStore/MusicStore.csproj", "MusicStore/"]

RUN dotnet restore "MusicStore/MusicStore.csproj"

COPY . .

WORKDIR "/src/MusicStore"
RUN dotnet publish "MusicStore.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:$PORT

ENTRYPOINT ["dotnet", "MusicStore.dll"]
