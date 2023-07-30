FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Carts/Carts.API/Carts.API.csproj", "src/Carts/Carts.API/"]
RUN dotnet restore "src/Carts/Carts.API/Carts.API.csproj"
COPY . .
WORKDIR "/src/src/Carts/Carts.API"
RUN dotnet build "Carts.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Carts.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Carts.API.dll"]
