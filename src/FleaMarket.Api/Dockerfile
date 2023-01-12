FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/FleaMarket.Api/FleaMarket.Api.csproj", "src/FleaMarket.Api/"]
RUN dotnet restore "src/FleaMarket.Api/FleaMarket.Api.csproj"
COPY . .
WORKDIR "/src/src/FleaMarket.Api"
RUN dotnet build "FleaMarket.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FleaMarket.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FleaMarket.Api.dll"]
