FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MovieCatalog.View/MovieCatalog.View.csproj", "MovieCatalog.View/"]
COPY ["MovieCatalog.Domain/MovieCatalog.Domain.csproj", "MovieCatalog.Domain/"]
COPY ["MovieCatalog.Infrastructure/MovieCatalog.Infrastructure.csproj", "MovieCatalog.Infrastructure/"]
RUN dotnet restore "MovieCatalog.View/MovieCatalog.View.csproj"
COPY . .
WORKDIR "/src/MovieCatalog.View"
RUN dotnet build "MovieCatalog.View.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MovieCatalog.View.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieCatalog.View.dll"]
