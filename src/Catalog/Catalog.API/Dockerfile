FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Catalog/Catalog.API/Catalog.API.csproj", "src/Catalog/Catalog.API/"]
RUN dotnet restore "src/Catalog/Catalog.API/Catalog.API.csproj"
COPY . .
WORKDIR "/src/src/Catalog/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Catalog.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
