#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=https://+:8080
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["lancer-resources-backend.csproj", "."]
RUN dotnet restore "./lancer-resources-backend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "lancer-resources-backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "lancer-resources-backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "lancer-resources-backend.dll", "--server.urls", "http://+:8080"]