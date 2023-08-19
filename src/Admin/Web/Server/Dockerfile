FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-preview AS build
ARG configuration=Release
WORKDIR /
COPY ["src/Admin/Web/Server/YourBrand.Server.csproj", "src/Admin/Web/Server/"]
COPY ["NuGet.config", "/"]
RUN dotnet restore "src/Admin/Web/Server/YourBrand.Server.csproj"
COPY . .
RUN dotnet test "/src/Admin/Web/Server.UnitTests"
WORKDIR "/src/Admin/Web/Server"
RUN dotnet build "YourBrand.Server.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "YourBrand.Server.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YourBrand.Server.dll"]
