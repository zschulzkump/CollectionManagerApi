FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER root
RUN apt-get -y update && apt-get install -y curl ca-certificates && \
    rm -rf /var/lib/apt/lists/*  
COPY ["./certs/","/app-certs"]
COPY ["./certs/collectionmanager-root-ca.crt","/usr/local/share/ca-certificates/"]
RUN update-ca-certificates

USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CollectionManagerApi/CollectionManagerApiCollectionManagerApi.csproj", "CollectionManagerApi/"]
RUN dotnet restore "CollectionManagerApi/CollectionManagerApi.csproj"
COPY . .
WORKDIR "/src/CollectionManagerApi"
RUN dotnet build "./CollectionManagerApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CollectionManagerApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CollectionManagerApi.dll"]
