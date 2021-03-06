#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AlzaProductApi/AlzaProductApi.csproj", "AlzaProductApi/"]
COPY ["AlzaProductApi.Data/AlzaProductApi.Data.csproj", "AlzaProductApi.Data/"]
COPY ["AlzaProductApi.Business/AlzaProductApi.Business.csproj", "AlzaProductApi.Business/"]
RUN dotnet restore "AlzaProductApi/AlzaProductApi.csproj"
COPY . .
WORKDIR "/src/AlzaProductApi"
RUN dotnet build "AlzaProductApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AlzaProductApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AlzaProductApi.dll"]
