FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-preview AS build
ARG configuration=Release
WORKDIR /
COPY ["src/Store/Web/Server/BlazorApp.csproj", "src/Store/Web/Server/"]
RUN dotnet restore "src/Store/Web/Server/BlazorApp.csproj"
COPY . .
RUN dotnet test "/src/Store/Web/Server.UnitTests"
WORKDIR "/src/Store/Web/Server"
RUN dotnet build "BlazorApp.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "BlazorApp.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorApp.dll"]
