FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DevBoard.slnx", "."]
COPY ["src/DevBoard.Api/DevBoard.Api.csproj", "src/DevBoard.Api/"]
COPY ["src/DevBoard.Infrastructure/DevBoard.Infrastructure.csproj", "src/DevBoard.Infrastructure/"]
COPY ["src/DevBoard.SharedKernel/DevBoard.SharedKernel.csproj", "src/DevBoard.SharedKernel/"]
COPY ["src/DevBoard.Modules.Projects/DevBoard.Modules.Projects.csproj", "src/DevBoard.Modules.Projects/"]
COPY ["src/DevBoard.Modules.Tasks/DevBoard.Modules.Tasks.csproj", "src/DevBoard.Modules.Tasks/"]
COPY ["src/DevBoard.Modules.Notifications/DevBoard.Modules.Notifications.csproj", "src/DevBoard.Modules.Notifications/"]
RUN dotnet restore "src/DevBoard.Api/DevBoard.Api.csproj"
COPY . .
WORKDIR "/src/src/DevBoard.Api"
RUN dotnet build "DevBoard.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DevBoard.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevBoard.Api.dll"]
