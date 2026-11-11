FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["EpoxyFloorManager.csproj", "./"]
RUN dotnet restore "EpoxyFloorManager.csproj"

COPY . .
RUN dotnet publish "EpoxyFloorManager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EpoxyFloorManager.dll"]
